using System.Security.Claims;
using TrainingBE.Common;
using TrainingBE.DTO;
using TrainingBE.Model;
using BCryptNet = BCrypt.Net.BCrypt;
using TrainingBE.Repository;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Numerics;

namespace TrainingBE.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<CustomerStatisticDTO> GetCustomerRevenues()
        {
            var customerRevenues = _unitOfWork.OrderRepository
        .GetAll()
        .Where(order => order.orderStatus == OrderStatus.DONE)
        .GroupBy(order => order.UserID)
        .Select(group => new CustomerStatisticDTO
        {
            id = group.Key,
            TotalRevenue = group.Sum(order => order.Total)
        })
        .ToList();

            foreach (var customerRevenue in customerRevenues)
            {
                var user = _unitOfWork.UserRepository.GetById(customerRevenue.id);
                if (user != null)
                {
                    customerRevenue.name = user.name;
                    customerRevenue.status = user.status;
                    customerRevenue.email = user.email;
                    customerRevenue.numberPhone = user.numberPhone;
                }
            }
            return customerRevenues;
        }
        public int GetUserIdFromClaims(ClaimsPrincipal userClaims)
        {
            Claim userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User Id not found in claims.");
        }
        public bool ChangePassword(int userId, ChangePasswordRequest request)
        {
            User user = _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                return false; 
            }

           


            var oldPasswordHash = BCryptNet.HashPassword(request.OldPassword);

           
            var verifyOldPasswordResult = BCryptNet.Verify(user.password, oldPasswordHash);

            if (verifyOldPasswordResult)
            {
                return false; 
            }
            if (!ValidatePassword(request.NewPassword))
            {
                return false;
            }
            if (request.NewPassword != request.ConfirmPassword)
            {
                return false; 
            }

            var hashedNewPassword = BCryptNet.HashPassword(request.NewPassword);
            user.password = hashedNewPassword;

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            return true;
        }
        public bool ValidatePassword(string password)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=])(?=.*[^\da-zA-Z]).{8,}$";
            return Regex.IsMatch(password, passwordPattern);
        }

        public bool ValidateUserData(UpdateProfileUserDTO updateProfileUserDTO, out string errorMessage)
        {
            errorMessage = "";
            

            if (updateProfileUserDTO.UserName.Contains(" "))
            {
                errorMessage = "Username must not contain spaces.";
                return false;
            }

            if (updateProfileUserDTO.UserName.Length < 3)
            {
                errorMessage = "Username must be at least 3 characters.";
                return false;
            }
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (!Regex.IsMatch(updateProfileUserDTO.Email, emailPattern))
            {
                errorMessage = "Invalid email format.";
                return false;
            }

            string phonePattern = @"^(?=.*[0-9])[- +()0-9]{10,12}$";
            if (!Regex.IsMatch(updateProfileUserDTO.PhoneNumber, phonePattern))
            {
                errorMessage = "Invalid phone number format. Phone number from 10 to 12 numbers.";
                return false;
            }
            return true;
        }
        public bool UpdateProfile(int userId, UpdateProfileUserDTO updateProfileUserDTO, out string errorMessage)
        {
            
            User user = _unitOfWork.UserRepository.GetById(userId);

            if (user == null)
            {
                errorMessage = "User not found.";
                return false;
            }
            if (!ValidateUserData(updateProfileUserDTO, out errorMessage))
            {
                return false;
            }
            var existingUser = _unitOfWork.UserRepository.GetUserByUsername(updateProfileUserDTO.UserName);
            if (existingUser != null && existingUser.id != userId)
            {
                errorMessage = "Username already exists. Please choose another username.";
                return false;
            }
            user.userName = updateProfileUserDTO.UserName;
            user.userName = updateProfileUserDTO.UserName; 
            user.name = updateProfileUserDTO.Name;   
            user.email = updateProfileUserDTO.Email; 
            user.numberPhone = updateProfileUserDTO.PhoneNumber;
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            errorMessage = string.Empty;
            return true;
        }

        public UserDTO GetUserDetails(int userId)
        {
            User user = _unitOfWork.UserRepository.GetById(userId);
            return  new UserDTO
            {
                Name = user.name,
                UserName = user.userName,
                Email = user.email,
                NumberPhone = user.numberPhone,
                Status = user.status,
                Role = user.role,
            };
        }

        public List<UserDTO> GetAllUser()
        {
            List<User> listUser = _unitOfWork.UserRepository.GetAll().ToList();
            List<UserDTO> listUserDTO = listUser.Select(user => new UserDTO
            {
               Name=user.name,
               UserName=user.userName,
               Email=user.email,
               NumberPhone=user.numberPhone,
               Status=user.status,
               Role=user.role,   
            }).ToList();
            return listUserDTO;        
        }
    }
}
