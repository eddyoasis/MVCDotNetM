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
using System.Text;

namespace MVCWebApp.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipient, string subject, string body);
    }

    public class EmailService(IOptionsSnapshot<SmtpAppSetting> _smtpAppSetting) : IEmailService
    {
        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            var smtpAppSetting = _smtpAppSetting.Value;

            // --- Email Configuration ---
            string smtpHost = smtpAppSetting.Host;
            string senderEmail = smtpAppSetting.EmailFrom;
            //string recipientEmail = recipient;
            var recipientEmails = smtpAppSetting.EmailTo;
            string emailSubject = subject;
            string strMailBody = body;

            try
            {
                // Use 'using' statements to ensure proper disposal of MailMessage and SmtpClient
                // This is the modern and recommended way to handle IDisposable objects
                using MailMessage mailMessage = new();
                // Set sender and recipient addresses
                mailMessage.From = new MailAddress(senderEmail);

                mailMessage.To.AddRange(recipientEmails.Select(e => new MailAddress(e)));
                //recipientEmails.ForEach(x => mailMessage.To.Add(x));
                //mailMessage.To.Add(recipientEmails);

                // Set subject (optional)
                mailMessage.Subject = emailSubject;

                // Configure email body
                mailMessage.IsBodyHtml = true; // Set to true if your body content is HTML
                mailMessage.Priority = MailPriority.High; // Set email priority
                mailMessage.Body = strMailBody; // Assign the body content

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
