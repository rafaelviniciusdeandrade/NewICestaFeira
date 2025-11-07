using System.Net;
using System.Net.Mail;

namespace CestaFeira.Web.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailSettings");
            _smtpHost = emailConfig["SmtpHost"];
            _smtpPort = int.Parse(emailConfig["SmtpPort"]);
            _fromEmail = emailConfig["FromEmail"];
            _fromPassword = emailConfig["FromPassword"];
            _fromName = emailConfig["FromName"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                using (var mailMessage = new MailMessage(_fromEmail, toEmail, subject, message))
                {
                    mailMessage.IsBodyHtml = true; // Se o corpo for HTML
                    mailMessage.From = new MailAddress(_fromEmail, _fromName);

                    using (var smtpClient = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(_fromEmail, _fromPassword);

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Trate ou registre o erro de envio de e-mail
                Console.WriteLine($"Erro ao enviar email para {toEmail}: {ex.Message}");
                // Você pode querer relançar a exceção ou apenas registrar e continuar
            }
        }
    }
}
