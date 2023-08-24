using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
        void SendEmailsToUsers(List<User> users);
        void SendResetPasswordEmail(string email, string resetToken);
    }
}
