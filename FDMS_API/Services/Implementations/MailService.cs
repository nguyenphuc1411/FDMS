using FDMS_API.Models.RequestModel;
using FDMS_API.Services.Interfaces;
using MimeKit;
using System.Net.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace FDMS_API.Services.Implementations
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_config["Smtp:FromName"], _config["Smtp:User"]));
                message.To.Add(new MailboxAddress("", mailRequest.ToEmail));
                message.Subject = mailRequest.Subject;
                message.Body = new TextPart("html")
                {
                    Text = mailRequest.Body
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_config["Smtp:Server"], int.Parse(_config["Smtp:Port"]), MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Pass"]);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error sending email: {smtpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
