using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using AccreditationApp.Models;

namespace AccreditationApp.Services
{
    public interface IEmailService
    {
        Task SendVerificationCodeAsync(string email, string code);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendVerificationCodeAsync(string email, string code)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = "Votre code de vérification - Banque de France",
                    Body = $@"
                    <h3>Banque de France - Code de Vérification</h3>
                    <p>Votre code de vérification est : <strong>{code}</strong></p>
                    <p>Ce code expirera dans 10 minutes.</p>
                    <br>
                    <p><em>Ne partagez jamais ce code avec personne.</em></p>
                    ",
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}