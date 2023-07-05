using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using static TrainingBE.Model.User;
using static TrainingBE.UserHelpers;
using System.Text;
using System.Security.Cryptography;
using TrainingBE.Model;

namespace TrainingBE.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : ControllerBase
    {
        public static List<User> list = new List<User>();
        [HttpGet]
        [Route("[controller]")]
        public IActionResult Index()
        {
            return Ok(list);
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(string userName, string password, string email, string numberPhone)
        {

            // check khoang trang
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(numberPhone))
            {
                Console.WriteLine("Information must not be empty or contain whitespace");
                return BadRequest("hello");
            }
            if (userName.Contains(" ") || password.Contains(" ") || email.Contains(" ") || numberPhone.Contains(" "))
            {
                return BadRequest("No spaces allowed.");
            }
            // user.userName
            if (userName.Length <= 3)
            {
                return BadRequest("Username must be more than 3 characters.");
            }
            //pasword
            if ((password.Length < 8) || (!password.Any(char.IsUpper)) || (!password.Any(char.IsLower)) || (!password.Any(char.IsDigit)))
            {

                return BadRequest("Password must be > 8 characters, including uppercase, lowercase, digit, and special characters.");
            }
            //sdt
            if (!numberPhone.All(char.IsDigit))
            {
                return BadRequest("NumberPhone must not contain letters.");
            }
            // user.email
            if (!email.Contains("@") || !email.Contains("."))
            {
                return BadRequest("Email format must be abc@gmail.com.");
            }
            if (numberPhone.Length < 10 || numberPhone.Length > 11)
            {
                return BadRequest("NumberPhone must be between 10 and 11 digits.");
            }
            UserHelpers userHelpers = new UserHelpers();
            string hashPassword = userHelpers.HashPassword(password);
            User user = new User(userName, hashPassword,UserStatus.active,email, numberPhone);
            list.Add(user);
            return new OkObjectResult(new
            {
                message ="Register succesfully",
                data=list,
            });
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string enteredUsername, string enteredPassword,UserStatus status)
        {
            if (string.IsNullOrWhiteSpace(enteredUsername) || string.IsNullOrWhiteSpace(enteredPassword))
            {
                return BadRequest("Username and password are required.");
            }
            if (status == UserStatus.inactive) {
                return BadRequest("This account is inactive.");
            }
            if (status == UserStatus.delete) {
                return BadRequest("thiss account is deleted");
            }
            
            User user = list.FirstOrDefault(u => u.userName == enteredUsername);
            if (user == null)
            {      
                return BadRequest("Username does not exist");
            }
            UserHelpers userHelpers = new UserHelpers();
            bool isPasswordValid = userHelpers.VerifyPassword(enteredPassword, user.password);
            if (!isPasswordValid)
            {
                return BadRequest("Invalid password");
            }
            return Ok("Login successful");
        }

        
    }

}
