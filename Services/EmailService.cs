using Google.Apis.Gmail.v1.Data;
using Microsoft.Extensions.Options;
using MimeKit;
using MVCWebApp.Configurations;
using Newtonsoft.Json;
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
            string recipientEmail = recipient;
            string emailSubject = subject;
            string strMailBody = body;

            try
            {
                // Use 'using' statements to ensure proper disposal of MailMessage and SmtpClient
                // This is the modern and recommended way to handle IDisposable objects
                using MailMessage mailMessage = new();
                // Set sender and recipient addresses
                mailMessage.From = new MailAddress(senderEmail);
                mailMessage.To.Add(recipientEmail);

                // Set subject (optional)
                mailMessage.Subject = emailSubject;

                // Configure email body
                mailMessage.IsBodyHtml = true; // Set to true if your body content is HTML
                mailMessage.Priority = MailPriority.High; // Set email priority
                mailMessage.Body = strMailBody; // Assign the body content

                using SmtpClient smtpClient = new(smtpHost);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email sent successfully!");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        //public async Task SendEmailAsync6(string recipient, string subject, string body)
        //{
        //    try
        //    {
        //        var accessToken = "sss";

        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("System", "SystemTemp@gmail.com"));
        //        message.To.Add(new MailboxAddress("Recipient", "eddy.wang@kgi.com"));
        //        message.Subject = "Hello from Gmail API";

        //        var builder = new BodyBuilder
        //        {
        //            TextBody = "This is a plain text email.",
        //            HtmlBody = "<p>This is an <strong>HTML</strong> email.</p>"
        //        };

        //        message.Body = builder.ToMessageBody();

        //        string rawMessage;
        //        using (var stream = new MemoryStream())
        //        {
        //            message.WriteTo(stream);
        //            rawMessage = Convert.ToBase64String(stream.ToArray())
        //                .Replace("+", "-")
        //                .Replace("/", "_")
        //                .Replace("=", "");
        //        }

        //        var httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        var payload = new
        //        {
        //            raw = rawMessage
        //        };

        //        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        //        var response = await httpClient.PostAsync("https://gmail.googleapis.com/gmail/v1/users/me/messages/send", content);
        //    }
        //    catch (Exception ex)
        //    {
        //        var ssss = ex;
        //    }
        //}
    }
}
