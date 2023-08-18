using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingBE.Data;
using TrainingBE.Model;
using TrainingBE.Repository;
using TrainingBE.Repository_Linq;

namespace TestProject1.Repository
{
    [TestFixture]
    internal class UserRepositoryTest
    {
        private DbContextOptions<MyDBContext> _dbContextOptions;
        private UserRepository _userRepository;
        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _userRepository = new UserRepository(new MyDBContext(_dbContextOptions));
            using (var context = new MyDBContext(_dbContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }
        [Test]
        public void GetUserByUsername_ReturnsUserIfExists()
        {
            // Arrange
            var testUsername = "testuser";
            var user = new User { id = 1, userName = testUsername, password = "testpassword",email="test@gmail.com",name="John",numberPhone="0123456456",
                role=User.Role.User,status=User.UserStatus.active };
            _userRepository.Add(user);
            _userRepository.Save();

            // Act
            var result = _userRepository.GetUserByUsername(testUsername);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUsername, result.userName);
        }
        [Test]
        public void CreateUser()
        {
            var testUsername = "testuser";
            var user = new User
            {
                id = 1,
                userName = testUsername,
                password = "testpassword",
                email = "test@gmail.com",
                name = "John",
                numberPhone = "0123456456",
                role = User.Role.User,
                status = User.UserStatus.active
            };
            _userRepository.Add(user);
            _userRepository.Save();
            var retrievedUser = _userRepository.GetUserByUsername(testUsername);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(testUsername, retrievedUser.userName);
        }
        [Test]
        public void UpdateUser_Successfully()
        {
            // Arrange
            var testUsername = "testuser";
            var user = new User
            {
                id = 1,
                userName = testUsername,
                password = "testpassword",
                email = "test@gmail.com",
                name = "John",
                numberPhone = "0123456456",
                role = User.Role.User,
                status = User.UserStatus.active
            };

            // Add the user to the repository
            _userRepository.Add(user);
            _userRepository.Save();

            // Change the user's information
            var updatedName = "Updated Name";
            var updatedEmail = "updated@test.com";
            var updatedStatus = User.UserStatus.inactive;
            user.name = updatedName;
            user.email = updatedEmail;
            user.status = updatedStatus;

            // Act
            _userRepository.Update(user);
            _userRepository.Save();

            // Get the updated user from the repository
            var updatedUser = _userRepository.GetUserByUsername(testUsername);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(updatedName, updatedUser.name);
            Assert.AreEqual(updatedEmail, updatedUser.email);
            Assert.AreEqual(updatedStatus, updatedUser.status);
        }
    }
}
