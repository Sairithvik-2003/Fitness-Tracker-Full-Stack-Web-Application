using FitnessTracker.BLRepository;
using FitnessTracker.Controllers;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrackerTests.ControllerTests
{
    public class UserControllerTest
    {
        private readonly Mock<IUserBLRepository> mockRepo;
        private readonly List<UserProfile> users;

        public UserControllerTest()
        {
            mockRepo = new Mock<IUserBLRepository>();

            users = new List<UserProfile>
            {
                new() { Id = 1, FullName = "Alice", Email = "alice@example.com", Gender = "Female", Weight = 60.0, Height = 165.5, Password = "password1", DateOfBirth = new DateOnly(1990, 1, 1) },
                new() { Id = 2, FullName = "Bob", Email = "bob@example.com", Gender = "Male", Weight = 75.0, Height = 175.0, Password = "password2", DateOfBirth = new DateOnly(1985, 5, 15) }
            };
        }

        // Test cases for AddNewUser

        /*[Fact]
        public async Task TestAddNewUser_ValidData()
        {
            // Arrange
            var newUser = new UserProfile { Id = 3, FullName = "Charlie", Email = "charlie@example.com", Gender = "Male", Weight = 85.0, Height = 180.0, Password = "password3", DateOfBirth = new DateOnly(1992, 8, 25) };
            mockRepo.Setup(x => x.AddNewUser(newUser)).ReturnsAsync(newUser);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.AddNewUser("Charlie", "charlie@example.com", "password3", "Male", 85.0, 180.0, "1992-08-25");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserProfile>(okResult.Value);
            Assert.Equal(newUser.FullName, returnValue.FullName);
        }*/


        [Fact]
        public async Task TestAddNewUser_InvalidData()
        {
            // Arrange
            var newUser = new UserProfile { Id = 3, FullName = "", Email = "", Gender = "Male", Weight = 85.0, Height = 180.0, Password = "password3", DateOfBirth = new DateOnly(1992, 8, 25) };
            mockRepo.Setup(x => x.AddNewUser(newUser)).ReturnsAsync(newUser);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.AddNewUser("", "", "password3", "Male", 85.0, 180.0, "1992-08-25");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        // Test cases for GetUserDetailsByAuthentication

        [Fact]
        public async Task TestGetUserDetailsByAuthentication_ValidCredentials()
        {
            // Arrange
            var email = "alice@example.com";
            var password = "password1";
            var loginDetails = new Login { UserId = 1, Email = email, Fullname = "Alice" };
            mockRepo.Setup(x => x.GetUserDetailsByAuthentication(email, password)).ReturnsAsync(loginDetails);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.GetUserDetailsByAuthentication(email, password);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Login>(okResult.Value);
            Assert.Equal(email, returnValue.Email);
        }

        [Fact]
        public async Task TestGetUserDetailsByAuthentication_InvalidCredentials()
        {
            // Arrange
            var email = "invalid@example.com";
            var password = "invalidpassword";
            mockRepo.Setup(x => x.GetUserDetailsByAuthentication(email, password)).ReturnsAsync((Login)null);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.GetUserDetailsByAuthentication(email, password);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        // Test cases for GetUserProfileByID

        [Fact]
        public async Task TestGetUserProfileByID_ValidId()
        {
            // Arrange
            var id = 1;
            var userDetails = new UserDetails { Id = 1, FullName = "Alice", Email = "alice@example.com", Gender = "Female", Weight = 60.0, Height = 165.5, DateOfBirth = new DateOnly(1990, 1, 1) };
            mockRepo.Setup(x => x.GetUserProfileByID(id)).ReturnsAsync(userDetails);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.GetUserProfileByID(id);


            var okResult = Assert.IsType<ActionResult<UserDetails>>(result);
            var returnValue = Assert.IsType<UserDetails>(okResult.Value);
            Assert.Equal(userDetails.FullName, returnValue.FullName);
        }

        [Fact]
        public async Task TestGetUserProfileByID_InvalidId()
        {
            // Arrange
            var id = 99; // Assuming this user ID does not exist
            mockRepo.Setup(x => x.GetUserProfileByID(id)).ReturnsAsync((UserDetails)null);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.GetUserProfileByID(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        // Test cases for DeleteUser

        [Fact]
        public async Task TestDeleteUser_ValidUserId()
        {
            // Arrange
            var userId = 1;
            mockRepo.Setup(x => x.DeleteUser(userId)).ReturnsAsync(true);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnValue);
        }

        [Fact]
        public async Task TestDeleteUser_InvalidUserId()
        {
            // Arrange
            var userId = 99; // Assuming this user ID does not exist
            mockRepo.Setup(x => x.DeleteUser(userId)).ReturnsAsync(false);

            // Act
            var controller = new UserController(mockRepo.Object);
            var result = await controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.False(returnValue);
        }
    }
}
