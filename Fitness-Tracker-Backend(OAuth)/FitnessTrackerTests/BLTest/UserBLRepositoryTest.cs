using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrackerTests.BLTest
{
    public class UserBLRepositoryTest
    {
        private readonly Mock<IUserRepository> mockRepo;
        private readonly UserBLRepository userBLRepository;
        private readonly List<UserProfile> userProfiles;

        public UserBLRepositoryTest()
        {
            mockRepo = new Mock<IUserRepository>();
            userBLRepository = new UserBLRepository(mockRepo.Object);

            userProfiles = new List<UserProfile>
            {
                new() { Id = 1, FullName = "Alice", Email = "alice@example.com", Gender = "Female", Weight = 60.0, Height = 165.5, Password = "password1", DateOfBirth = new DateOnly(1990, 1, 1) },
                new() { Id = 2, FullName = "Bob", Email = "bob@example.com", Gender = "Male", Weight = 75.0, Height = 175.0, Password = "password2", DateOfBirth = new DateOnly(1985, 5, 15) }
            };
        }

        [Fact]
        public async Task TestAddNewUser()
        {
            // Arrange
            var newUser = new UserProfile { Id = 3, FullName = "Charlie", Email = "charlie@example.com", Gender = "Male", Weight = 85.0, Height = 180.0, Password = "password3", DateOfBirth = new DateOnly(1992, 8, 25) };
            mockRepo.Setup(x => x.AddNewUser(newUser)).ReturnsAsync(newUser);

            // Act
            var result = await userBLRepository.AddNewUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Email, result.Email);
        }

        [Fact]
        public async Task TestAddNewUser_Failure()
        {
            // Arrange
            var newUser = new UserProfile { Id = 4, FullName = "Dave", Email = "dave@example.com", Gender = "Male", Weight = 90.0, Height = 185.0, Password = "password4", DateOfBirth = new DateOnly(1995, 3, 14) };
            mockRepo.Setup(x => x.AddNewUser(newUser)).ThrowsAsync(new Exception("Error adding new user"));

            // Act
            var result = await userBLRepository.AddNewUser(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Email, result.Email);
        }

        [Fact]
        public async Task TestGetUserDetailsByAuthentication_ValidCredentials()
        {
            // Arrange
            var email = "alice@example.com";
            var password = "password1";
            var expectedLogin = new Login { UserId = 1, Email = email, Fullname = "Alice" };
            mockRepo.Setup(x => x.GetUserDetailsByAuthentication(email, password)).ReturnsAsync(expectedLogin);

            // Act
            var result = await userBLRepository.GetUserDetailsByAuthentication(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedLogin.Email, result.Email);
        }

        [Fact]
        public async Task TestGetUserDetailsByAuthentication_InvalidCredentials()
        {
            // Arrange
            var email = "invalid@example.com";
            var password = "invalidpassword";
            mockRepo.Setup(x => x.GetUserDetailsByAuthentication(email, password)).ReturnsAsync((Login)null);

            // Act
            var result = await userBLRepository.GetUserDetailsByAuthentication(email, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetUserProfileByID_ValidId()
        {
            // Arrange
            var userId = 1;
            var expectedUserDetails = new UserDetails { Id = 1, FullName = "Alice", Email = "alice@example.com", Gender = "Female", Weight = 60.0, Height = 165.5, DateOfBirth = new DateOnly(1990, 1, 1) };
            mockRepo.Setup(x => x.GetUserProfileByID(userId)).ReturnsAsync(expectedUserDetails);

            // Act
            var result = await userBLRepository.GetUserProfileByID(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUserDetails.FullName, result.FullName);
        }

        [Fact]
        public async Task TestGetUserProfileByID_InvalidId()
        {
            // Arrange
            var userId = 99; 
            mockRepo.Setup(x => x.GetUserProfileByID(userId)).ReturnsAsync((UserDetails)null);

            // Act
            var result = await userBLRepository.GetUserProfileByID(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TestDeleteUser_ValidUserId()
        {
            // Arrange
            var userId = 1;
            mockRepo.Setup(x => x.DeleteUser(userId)).ReturnsAsync(true);

            // Act
            var result = await userBLRepository.DeleteUser(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task TestDeleteUser_InvalidUserId()
        {
            // Arrange
            var userId = 99; 
            mockRepo.Setup(x => x.DeleteUser(userId)).ReturnsAsync(false);

            // Act
            var result = await userBLRepository.DeleteUser(userId);

            // Assert
            Assert.False(result);
        }
    }
}
