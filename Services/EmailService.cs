using FluentEmail.Core;
using Google.Apis.Gmail.v1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MVCWebApp.Configurations;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace MVCWebApp.Services
{
    public interface IEmailService
    {
        //Task SendEmailAsync(string recipient, string subject, string body);
        Task SendEmailAsync(List<string> recipientsTo, List<string> recipientsCC, string subject, string body);
    }

    public class EmailService(
        IOptionsSnapshot<SmtpAppSetting> _smtpAppSetting,
        IOptionsSnapshot<DocumentAppSetting> _documentAppSetting
        ) : IEmailService
    {
        public async Task SendEmailAsync(List<string> recipientsTo, List<string> recipientsCC, string subject, string body)
        {
            var smtpAppSetting = _smtpAppSetting.Value;
            var documentAppSetting = _documentAppSetting.Value;

            // --- Email Configuration ---
            string smtpHost = smtpAppSetting.Host;
            string senderEmail = smtpAppSetting.EmailFrom;
            //string recipientEmail = recipient;
            //var recipientEmails = smtpAppSetting.EmailTo;
            string emailSubject = subject;
            string strMailBody = body;

            try
            {
                using MailMessage mailMessage = new();

                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.To.AddRange(recipientsTo.Select(e => new MailAddress(e)));

                if(recipientsCC.Any())
                {
                    mailMessage.CC.AddRange(recipientsCC.Select(e => new MailAddress(e)));
                }

                // Attach the file
                if (File.Exists(documentAppSetting.DocumentPath))
                {
                    Attachment attachment = new Attachment(documentAppSetting.DocumentPath, MediaTypeNames.Application.Octet);
                    mailMessage.Attachments.Add(attachment);
                }

                // Set subject (optional)
                mailMessage.Subject = emailSubject;

                // Configure email body
                //mailMessage.IsBodyHtml = true; // Set to true if your body content is HTML
                mailMessage.IsBodyHtml = false;
                mailMessage.Priority = MailPriority.High; // Set email priority
                //mailMessage.Body = strMailBody; // Assign the body content

                string formattedMessage = strMailBody.Replace("    ", Environment.NewLine + Environment.NewLine);
                mailMessage.Body = formattedMessage;

                using SmtpClient smtpClient = new(smtpHost);

                Log.Information("➡️ START: EmailService.SendEmailAsync -> {req}",
                    JsonConvert.SerializeObject(smtpAppSetting, Formatting.Indented));

                smtpClient.Send(mailMessage);
                Log.Information("✅ END: EmailService.SendEmailAsync -> Email sent successfully!");
            }
            catch (SmtpException ex)
            {
                Log.Information("✅ END Error: EmailService.SendEmailAsync -> Error {ex}",
                    JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Log.Information("✅ END Error: EmailService.SendEmailAsync -> Error {ex}",
                    JsonConvert.SerializeObject(ex, Formatting.Indented));
            }
        }
    }
}
