using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Service;
using BCryptNet = BCrypt.Net.BCrypt;

namespace TestProject1.Service
{
    [TestFixture]
    internal class AuthServiceTest
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _authService = new AuthService(_mockUnitOfWork.Object,null);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsTrue()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = BCryptNet.HashPassword(password);
            var user = new User
            {
                id = 1,
                userName = username,
                password = hashedPassword,
                status = User.UserStatus.active
            };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _authService.Login(username, password, out string errorMessage);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(errorMessage);
        }
        [Test]
        public void Login_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";
            var hashedPassword = BCryptNet.HashPassword("differentpassword");
            var user = new User
            {
                id = 1,
                userName = username,
                password = hashedPassword,
                status = User.UserStatus.active
            };
            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByUsername(username)).Returns(user);

            // Act
            var result = _authService.Login(username, password, out string errorMessage);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Incorrect password.", errorMessage);
        }

        [Test]
        public void Register_ValidUser_CallsRepositoryAdd()
        {
            // Arrange
            var user = new User
            {
                id = 1,
                userName = "newuser",
                password = "newpassword",
                email = "newuser@test.com",
                name = "New User",
                numberPhone = "1234567890",
                role = User.Role.User,
                status = User.UserStatus.active
            };

            _mockUnitOfWork.Setup(uow => uow.UserRepository.GetUserByUsername(user.userName)).Returns((User)null);

            // Act
            _authService.Register(user);

            // Assert
            _mockUnitOfWork.Verify(uow => uow.UserRepository.Add(user), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Save(), Times.Once);
        }
    }
}
