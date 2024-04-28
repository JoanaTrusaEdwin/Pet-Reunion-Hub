using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace Pet_Reunion_Hub.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["Sender"]));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSettings["MailServer"], int.Parse(emailSettings["MailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSettings["Sender"], emailSettings["Password"]);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }

}

