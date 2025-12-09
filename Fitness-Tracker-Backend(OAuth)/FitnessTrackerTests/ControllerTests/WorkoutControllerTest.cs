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
    public class WorkoutControllerTest
    {
        private readonly Mock<IWorkoutBLRepository> mockRepo;
        private readonly List<Workout> workouts;

        public WorkoutControllerTest()
        {
            mockRepo = new Mock<IWorkoutBLRepository>();

            workouts = new List<Workout>
            {
                new() { WorkoutId = 1, WorkoutType = "Outdoor", WorkoutName = "Morning Run", Duration = new TimeOnly(1, 0, 0), Distance = 5.0, CaloriesBurned = 500, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 },
                new() { WorkoutId = 2, WorkoutType = "Indoor", WorkoutName = "Yoga", Duration = new TimeOnly(0, 45, 0), Distance = 0.0, CaloriesBurned = 200, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 }
            };
        }

        [Fact]
        public async Task TestGetWorkOutDetailsByUserID_ValidUserId()
        {
            // Arrange
            var userId = 1;
            mockRepo.Setup(x => x.GetWorkOutDetailsByUserID(userId)).ReturnsAsync(workouts);

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.GetWorkOutDetailsByUserID(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Workout>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }


        [Fact]
        public async Task TestAddNewActivityOfUser_ValidData()
        {
            // Arrange
            var newWorkout = new Workout
            {
                WorkoutId = 3,
                WorkoutType = "Indoor",
                WorkoutName = "Weight Lifting",
                Duration = new TimeOnly(1, 30, 0),
                Distance = 0.0,
                CaloriesBurned = 600,
                Date = DateOnly.FromDateTime(DateTime.Now),
                UserProfileId = 1
            };
            mockRepo.Setup(x => x.AddNewActivityOfUser(newWorkout)).ReturnsAsync(newWorkout);

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.AddNewActivityOfUser("Indoor", "Weight Lifting", "01:30", 0.0, 600, DateOnly.FromDateTime(DateTime.Now).ToString(), 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Workout>(okResult.Value);
            Assert.Equal(newWorkout.WorkoutName, returnValue.WorkoutName);
        }

        [Fact]
        public async Task TestAddNewActivityOfUser_InvalidData()
        {
            // Arrange
            var newWorkout = new Workout
            {
                WorkoutId = 3,
                WorkoutType = "Indoor",
                WorkoutName = "",
                Duration = new TimeOnly(1, 30, 0),
                Distance = 0.0,
                CaloriesBurned = 600,
                Date = DateOnly.FromDateTime(DateTime.Now),
                UserProfileId = 1
            };
            mockRepo.Setup(x => x.AddNewActivityOfUser(newWorkout)).ReturnsAsync(newWorkout);

            var controller = new WorkoutController(mockRepo.Object);
            controller.ModelState.AddModelError("WorkoutName", "Required");

            // Act
            var result = await controller.AddNewActivityOfUser("Indoor", "", "01:30", 0.0, 600, DateOnly.FromDateTime(DateTime.Now).ToString(), 1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task TestDeleteAndDisplayAll_ValidActivityId()
        {
            // Arrange
            var activityId = 1;
            mockRepo.Setup(x => x.DeleteAndDisplayAll(activityId)).ReturnsAsync(workouts);

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.DeleteAndDisplayAll(activityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Workout>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task TestDeleteAndDisplayAll_InvalidActivityId()
        {
            // Arrange
            var activityId = 99; 
            mockRepo.Setup(x => x.DeleteAndDisplayAll(activityId)).ReturnsAsync(new List<Workout>());

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.DeleteAndDisplayAll(activityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Workout>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public async Task TestGoalReachedOrNot_ValidData()
        {
            // Arrange
            var goalName = "Morning Run";
            var activityId = 1;
            var goalDetails = new WorkoutandFitnessGoal
            {
                ActivityType = "Outdoor",
                ActivityName = "Morning Run",
                Duration = new TimeOnly(1, 0, 0),
                Distance = 5.0,
                CaloriesBurned = 500,
                Date = DateOnly.FromDateTime(DateTime.Now),
                IsAchieved = true
            };
            mockRepo.Setup(x => x.GoalReachedOrNot(goalName, activityId)).ReturnsAsync(goalDetails);

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.GoalReachedOrNot(goalName, activityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<WorkoutandFitnessGoal>(okResult.Value);
            Assert.True(returnValue.IsAchieved);
        }

        [Fact]
        public async Task TestGoalReachedOrNot_InvalidData()
        {
            // Arrange
            var goalName = "InvalidGoal";
            var activityId = 99; // Assuming this activity ID does not exist
            mockRepo.Setup(x => x.GoalReachedOrNot(goalName, activityId)).ReturnsAsync(new WorkoutandFitnessGoal());

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.GoalReachedOrNot(goalName, activityId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<WorkoutandFitnessGoal>(okResult.Value);
            Assert.False(returnValue.IsAchieved);
        }


        [Fact]
        public async Task TestUpdateWorkoutByDetails_ValidData()
        {
            // Arrange
            var updatedWorkout = new Workout
            {
                WorkoutId = 3,
                WorkoutType = "Indoor",
                WorkoutName = "Yoga",
                Duration = new TimeOnly(1, 30, 0),
                Distance = 0.0,
                CaloriesBurned = 300,
                Date = DateOnly.FromDateTime(DateTime.Now),
                UserProfileId = 1
            };
            mockRepo.Setup(x => x.UpdateWorkoutByDetails(updatedWorkout)).ReturnsAsync(updatedWorkout);

            // Act
            var controller = new WorkoutController(mockRepo.Object);
            var result = await controller.UpdateWorkoutByDetails("Indoor", "Yoga", "01:30", 0.0, 300, DateOnly.FromDateTime(DateTime.Now).ToString(), 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Workout>(okResult.Value);
            Assert.Equal(updatedWorkout.WorkoutName, returnValue.WorkoutName);
        }

      

        


    }
}
