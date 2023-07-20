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
            if (isAuthenticated)
            {
                return Ok("Login successful");
            }

            return Unauthorized(errorMessage);
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
                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
