#region

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

#endregion

namespace CoreLib.ASP.Helpers.MailHelpers
{
    public class MailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public MailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using (var smtpClient = new SmtpClient
            {
                Host = _configuration.GetValue<string>("MailSender:SMTPServer"),
                Port = _configuration.GetValue<int>("MailSender:SMTPPort"),
                EnableSsl = false,
                Credentials = new NetworkCredential(_configuration.GetValue<string>("MailSender:Login"),
                    _configuration.GetValue<string>("MailSender:Password"))
            })
            {
                using (var message = new MailMessage(_configuration.GetValue<string>("MailSender:Login"), email)
                {
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                })
                {
                    await smtpClient.SendMailAsync(message);
                }
            }
        }

        public static string GenerateHtmlEmailMessageFromTemplate(string message, string pathToTemplate)
        {
            using (var sr = File.OpenText(pathToTemplate))
            {
                return sr.ReadToEnd().Replace("{Content}", message);
            }
        }
    }
}