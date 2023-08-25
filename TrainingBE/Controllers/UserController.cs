using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrainingBE.DTO;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController: ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public UserController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }
        [HttpPost("change-password")]
        [Authorize]
        public IActionResult ChangePassword([FromBody] ChangePasswordRequest request)
        {
            int userId = _userService.GetUserIdFromClaims(User); // Lấy userId từ claims
            if (!_userService.ValidatePassword(request.NewPassword))
            {
                return BadRequest("Password must have at least 8 characters, contain uppercase letters, lowercase letters, numbers and at least 1 special character.");
            }
            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest("Confirm password does not match.");
            }

            bool passwordChanged = _userService.ChangePassword(userId, request);

            if (passwordChanged)
            {
                return Ok("Password changed successfully.");
            }
            else
            {
                return BadRequest("Failed to change password.");
            }
        }
        [HttpPost("update-profile")]
        [Authorize]
        public IActionResult ChangeProfile([FromBody] UpdateProfileUserDTO request)
        {
            int userId = _userService.GetUserIdFromClaims(User); // Lấy userId từ claims
            string errorMessage;
            if (_userService.UpdateProfile(userId, request, out errorMessage))
            {
                return Ok("Profile updated successfully.");
            }
            else
            {
                return BadRequest(errorMessage);
            }
        }
        [HttpGet("user-details/{userId}")]
        [Authorize]
        public IActionResult GetUserDetails(int userId)
        {
            int currentUserId = _userService.GetUserIdFromClaims(User); // Lấy userId từ claims
            string currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value; // Lấy vai trò từ claims

            if (currentUserId != userId && currentUserRole != "Admin")
            {
                return Forbid();
            }
            var userDetails = _userService.GetUserDetails(userId);
            return Ok(userDetails);
        }
        [HttpGet("get-all-user")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUser()
        {
            var userDTO = _userService.GetAllUser();
            return Ok(userDTO);
        }
    }

}
