using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IAuthService
    {
        User getUserByID (int id);
        bool Login(string username, string password,out string errorMessage);
        void Register(User user);
        bool ValidateRegister(User registerRequest, out string errorMessage);
        
    }
}
