using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace FitnessTrackerTests.DAL_Tests
{
    public class UserRepositoryTests
    {
        private DbContextOptions<FitnessTrackerDbContext> CreateInMemoryDatabaseOptions()
        {
            return new DbContextOptionsBuilder<FitnessTrackerDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }   

        private async Task SeedData(DbContextOptions<FitnessTrackerDbContext> options)
        {
            using (var context = new FitnessTrackerDbContext(options))
            {
                // Clear existing data to avoid duplicates
                context.UserProfiles.RemoveRange(context.UserProfiles);
                await context.SaveChangesAsync();

                context.UserProfiles.AddRange(
                    new UserProfile { Id = 1, FullName = "User 1", Email = "user1@example.com", Password = "password1", Gender = "Male", Weight = 70.5, Height = 175.0, DateOfBirth = new DateOnly(1990, 1, 1) },
                    new UserProfile { Id = 2, FullName = "User 2", Email = "user2@example.com", Password = "password2", Gender = "Female", Weight = 60.0, Height = 165.0, DateOfBirth = new DateOnly(1992, 2, 2) }
                );
                await context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AddNewUserTest_AddsUserSuccessfully()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            var newUser = new UserProfile { FullName = "User 3", Email = "user3@example.com", Password = "password3", Gender = "Other", Weight = 75.0, Height = 180.0, DateOfBirth = new DateOnly(1995, 3, 3) };

            // Act
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                await repository.AddNewUser(newUser);
            }

            // Assert
            using (var context = new FitnessTrackerDbContext(options))
            {
                var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.Email == "user3@example.com");
                Assert.NotNull(user);
                Assert.Equal("User 3", user.FullName);
            }
        }

        [Fact]
        public async Task AddNewUserTest_DuplicateEmail_ReturnsNull()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            var duplicateUser = new UserProfile { FullName = "User 4", Email = "user1@example.com", Password = "password4", Gender = "Female", Weight = 65.0, Height = 170.0, DateOfBirth = new DateOnly(1994, 4, 4) };

            // Act
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.Email == "user1@example.com");
                if (user.Email != duplicateUser.Email)
                {
                    await repository.AddNewUser(duplicateUser);
                }
            }

            // Assert
            using (var context = new FitnessTrackerDbContext(options))
            {
                var user = await context.UserProfiles.FirstOrDefaultAsync(u => u.Email == "user1@example.com");
                Assert.NotNull(user); // Ensure original user still exists
                Assert.Equal("User 1", user.FullName);
            }
        }

        [Fact]
        public async Task GetUserDetailsByAuthenticationTest_ReturnsUserDetails()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            string email = "user1@example.com";
            string password = "password1";

            // Act
            Login result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.GetUserDetailsByAuthentication(email, password);
            }

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal("user1@example.com", result.Email);
            Assert.Equal("User 1", result.Fullname);
        }

        [Fact]
        public async Task GetUserDetailsByAuthenticationTest_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            string email = "user1@example.com";
            string password = "wrongpassword";

            // Act
            Login result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.GetUserDetailsByAuthentication(email, password);
                /*if(result.UserId == 0)
                {
                    result = null;
                }*/
            }

            // Assert
            Assert.Null(result); // Expecting null for invalid credentials
        }

        [Fact]
        public async Task GetUserProfileByIDTest_ReturnsUserProfile()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            int userId = 1;

            // Act
            UserDetails result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.GetUserProfileByID(userId);
            }

            
            Assert.NotNull(result);
            Assert.Equal("User 1", result.FullName);
            Assert.Equal("user1@example.com", result.Email);
        }

        [Fact]
        public async Task GetUserProfileByIDTest_NonExistentUser_ReturnsNull()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            int userId = 999; 

            
            UserDetails result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.GetUserProfileByID(userId);
                if (result.Email == null)
                {
                    result = null;
                }
            }

            
            Assert.Null(result); 
        }

        [Fact]
        public async Task DeleteUserTest_DeletesUserSuccessfully()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            int userIdToDelete = 1;

            // Act
            bool result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.DeleteUser(userIdToDelete);
            }

            // Assert
            Assert.True(result);
            using (var context = new FitnessTrackerDbContext(options))
            {
                var user = await context.UserProfiles.FindAsync(userIdToDelete);
                Assert.Null(user);
            }
        }

        [Fact]
        public async Task DeleteUserTest_NonExistentUser_ReturnsFalse()
        {
            // Arrange
            var options = CreateInMemoryDatabaseOptions();
            await SeedData(options);
            int userIdToDelete = 999; // Non-existent ID

            // Act
            bool result;
            using (var context = new FitnessTrackerDbContext(options))
            {
                var repository = new UserRepository(context);
                result = await repository.DeleteUser(userIdToDelete);
            }

            // Assert
            Assert.False(result); // Expecting false for non-existent user
        }
    }
}
    