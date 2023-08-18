using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Controllers;
using TrainingBE.Model;
using TrainingBE.Service;

namespace TestProject1.Controller
{
    [TestFixture]
    internal class AuthControllerTest
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _authController;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }
        [Test]
        public void Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var request = new LoginRequest
            {
                userName = "testuser",
                password = "testpassword"
            };
            _mockAuthService.Setup(authService => authService.Login(request.userName, request.password, out It.Ref<string>.IsAny)).Returns(true);

            // Act
            var result = _authController.Login(request) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login successful", result.Value);
        }

       
        [Test]
        public void Register_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var user = new User
            {
                name = "John",
                userName = "john123",
                password = "password123",
                status = User.UserStatus.active,
                email = "john@example.com",
                numberPhone = "1234567890",
                role = User.Role.User
            };
            _mockAuthService.Setup(authService => authService.ValidateRegister(user, out It.Ref<string>.IsAny)).Returns(true);

            // Act
            var result = _authController.Register(user) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Registration successful", result.Value);
        }
    }
}
