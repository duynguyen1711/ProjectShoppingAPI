using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TrainingBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            string errorMessage;
            bool isAuthenticated = _authService.Login(request.userName, request.password,out errorMessage);
            if (!isAuthenticated)
            {
                return Unauthorized(new { Error = errorMessage });
            }

            var user = _authService.GetUserByUsername(request.userName); // Chỉnh sửa tên phương thức để lấy thông tin người dùng dựa trên tên đăng nhập
            var token = _authService.GenerateJwtToken(user);

            return Ok(new { 
                Token = token,
                Message ="Login successfully"
            });
        }

        [HttpPost("register")]
        public IActionResult Register(User request)
        {
            string errorMessage;
            if (!_authService.ValidateRegister(request, out errorMessage))
            {
                return BadRequest(errorMessage);
            }
            var user = new User
            {
                name = request.name,
                userName = request.userName,
                password = request.password,
                status= request.status,
                email =request.email,
                numberPhone = request.numberPhone,
                role = request.role
                
            };

            try
            {
                _authService.Register(user);
                var token = _authService.GenerateJwtToken(user);
                return Ok(new { Message = "Registration successful", Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
