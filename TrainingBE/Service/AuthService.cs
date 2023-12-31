﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TrainingBE.Model;
using TrainingBE.Repository;
using BCryptNet = BCrypt.Net.BCrypt;
namespace TrainingBE.Service
{   
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private IEmailService _emailService;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration config, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _emailService = emailService;
        }

        public User getUserByID(int id)
        {
            return _unitOfWork.UserRepository.GetById(id);
        }

        public bool Login(string username, string password, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                errorMessage= "Please provide username and password.";
                return false;
            }
            var user = _unitOfWork.UserRepository.GetUserByUsername(username);

            if (user == null)
            {
                errorMessage = "Username does not exist.";
                return false;
            }
            bool isPasswordValid = BCryptNet.Verify(password, user.password);
            if (!isPasswordValid)
            {
                errorMessage = "Incorrect password.";
                return false;
            }

            if (user.status == User.UserStatus.inactive)
            {
                errorMessage = "Your account has been disabled. Please contact the administrator.";
                return false;
            }

            if (user.status == User.UserStatus.delete)
            {
                errorMessage = "Your account has been deleted. Please contact the administrator.";
                return false;
            }
            return true;
        }

        public void Register(User user)
        {
            string hashedPassword = BCryptNet.HashPassword(user.password);
            user.password = hashedPassword;
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Save();
        }
        public bool ValidateRegister(User registerRequest, out string errorMessage)
        {
            errorMessage = "";
            
            if (_unitOfWork.UserRepository.GetUserByUsername(registerRequest.userName) != null)
            {
                errorMessage = "This account has already existed. Please choose another account name.";
                return false;
            }
            // kiểm tra khoảng trắng 
            if (registerRequest.userName.Contains(" "))
            {
                errorMessage="Doesn't have whitespace";
                return false;
            }
            // Kiểm tra độ dài username
            if (registerRequest.userName.Length < 3)
            {
                errorMessage = "Username must be at least 3 characters.";
                return false;
            }
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&+=])(?=.*[^\da-zA-Z]).{8,}$";
            if (!Regex.IsMatch(registerRequest.password, passwordPattern))
            {
                errorMessage = "Password must have at least 8 characters, contain uppercase letters, lowercase letters, numbers and at least 1 special character.";
                return false;
            }

            // Kiểm tra định dạng email
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (!Regex.IsMatch(registerRequest.email, emailPattern))
            {
                errorMessage = "Invalid email format.";
                return false;
            }

            // Kiểm tra định dạng số điện thoại
            string phonePattern = @"^(?=.*[0-9])[- +()0-9]{10,12}$";
            if (!Regex.IsMatch(registerRequest.numberPhone, phonePattern))
            {
                errorMessage = "Invalid phone number format. Phone number from 10 to 12 numbers";
                return false;
            }
            
            errorMessage = string.Empty;
            return true;
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.userName),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", user.role))
            }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public User GetUserByUsername(string userName)
        {
            return _unitOfWork.UserRepository.GetUserByUsername(userName);
        }
        public bool RequestPasswordReset(string email)
        {
            User user = _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            
            string resetToken = GenerateResetToken(user.id);

           
            user.ResetToken = resetToken;
            user.ResetTokenExpiration = DateTime.UtcNow.AddMinutes(10); 
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            
            _emailService.SendResetPasswordEmail(user.email, resetToken);

            return true;
        }
        public bool ResetPassword(string email, string token, string newPassword)
        {
            User user = _unitOfWork.UserRepository.GetUserByEmail(email);
            if (user == null || user.ResetToken != token || user.ResetTokenExpiration <= DateTime.UtcNow)
            {
                return false;
            }

           
            user.password = BCryptNet.HashPassword(newPassword); 
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();
            return true;
        }

        private string GenerateResetToken(int id)
        {
            return Guid.NewGuid().ToString();
        }
    }
}
