using System.Security.Claims;
using TrainingBE.DTO;
using TrainingBE.Model;

namespace TrainingBE.Service
{
    public interface IUserService
    {
        List<CustomerStatisticDTO> GetCustomerRevenues();
        int GetUserIdFromClaims(ClaimsPrincipal userClaims);
        bool ChangePassword(int userId, ChangePasswordRequest request);
        bool ValidatePassword(string password);
        bool ValidateUserData(UpdateProfileUserDTO updateProfileUserDTO, out string errorMessage);
        bool UpdateProfile(int userId, UpdateProfileUserDTO updateProfileUserDTO, out string errorMessage);
        UserDTO GetUserDetails (int userId);
        List<UserDTO> GetAllUser();

    }
}
