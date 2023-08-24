using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using TrainingBE.Model;
using TrainingBE.Repository;
using static Org.BouncyCastle.Math.EC.ECCurve;
using MimeKit.Text;
using TrainingBE.DTO;

namespace TrainingBE.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SendEmail(EmailDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailService:EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["EmailService:EmailHost"], 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["EmailService:EmailUsername"], _configuration["EmailService:EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public void SendEmailsToUsers(List<User> users)
        {
            foreach (var user in users)
            {
                var emailBody = GetEmailBody(user);
                var emailRequest = new EmailDTO
                {
                    To = user.email,
                    Subject = "CHƯƠNG TRÌNH KHUYẾN MÃI",
                    Body = emailBody
                };
                SendEmail(emailRequest);
            }
        }

        public void SendResetPasswordEmail(string email, string resetToken)
        {
            var emailRequest = new EmailDTO
            {
                To = email,
                Subject = "RESET PASSWORD",
                Body = resetToken   
            };
            SendEmail(emailRequest);
        }

        private string GetEmailBody(User user)
        {
            return $"Hello {user.name},<br/><br/>Chào mưng quý khách đến chương trình khuyến mãi của chúng tôi";
        }



    }
}
