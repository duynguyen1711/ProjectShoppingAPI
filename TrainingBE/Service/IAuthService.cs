using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IAuthService
    {
        bool Login(string username, string password,out string errorMessage);
        void Register(User user);
        bool ValidateRegister(User registerRequest, out string errorMessage);
        
    }
}
