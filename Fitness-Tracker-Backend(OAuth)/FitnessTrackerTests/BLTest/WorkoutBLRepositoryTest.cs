using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrackerTests.BLTest
{
    public class WorkoutBLRepositoryTest
    {
        private readonly Mock<IWorkoutRepository> mockWorkoutRepo;
        private readonly Mock<IFitnessGoalBLRepository> mockFitnessGoalRepo;
        private readonly WorkoutBLRepository workoutBLRepository;
        private readonly List<Workout> workouts;
        private readonly List<FitnessGoal> fitnessGoals;

        public WorkoutBLRepositoryTest()
        {
            mockWorkoutRepo = new Mock<IWorkoutRepository>();
            mockFitnessGoalRepo = new Mock<IFitnessGoalBLRepository>();
            workoutBLRepository = new WorkoutBLRepository(mockWorkoutRepo.Object, mockFitnessGoalRepo.Object);

            workouts = new List<Workout>
            {
                new() { WorkoutId = 1, WorkoutType = "Outdoor", WorkoutName = "Morning Run", Duration = new TimeOnly(1, 0, 0), Distance = 5.0, CaloriesBurned = 500, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 },
                new() { WorkoutId = 2, WorkoutType = "Indoor", WorkoutName = "Yoga", Duration = new TimeOnly(0, 45, 0), Distance = 0.0, CaloriesBurned = 200, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 }
            };

            fitnessGoals = new List<FitnessGoal>
            {
                new() { FitnessGoalId = 1, GoalType = "Outdoor", GoalName = "Morning Run", Duration = new TimeOnly(1, 0, 0), Distance = 5.0, CaloriesBurned = 500 },
                new() { FitnessGoalId = 2, GoalType = "Indoor", GoalName = "Yoga", Duration = new TimeOnly(0, 45, 0), Distance = 0.0, CaloriesBurned = 200 }
            };
        }

        [Fact]
        public async Task TestAddNewActivityOfUser_ValidData()
        {
            // Arrange
            var newWorkout = new Workout { WorkoutId = 3, WorkoutType = "Indoor", WorkoutName = "Weight Lifting", Duration = new TimeOnly(1, 30, 0), Distance = 0.0, CaloriesBurned = 600, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 };
            mockWorkoutRepo.Setup(x => x.AddNewActivityOfUser(newWorkout)).ReturnsAsync(newWorkout);

            // Act
            var result = await workoutBLRepository.AddNewActivityOfUser(newWorkout);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newWorkout.WorkoutName, result.WorkoutName);
        }

        [Fact]
        public async Task TestAddNewActivityOfUser_Exception()
        {
            // Arrange
            var newWorkout = new Workout { WorkoutId = 3, WorkoutType = "Indoor", WorkoutName = "Weight Lifting", Duration = new TimeOnly(1, 30, 0), Distance = 0.0, CaloriesBurned = 600, Date = DateOnly.FromDateTime(DateTime.Now), UserProfileId = 1 };
            mockWorkoutRepo.Setup(x => x.AddNewActivityOfUser(newWorkout)).ThrowsAsync(new Exception("Error adding workout"));

            // Act
            var result = await workoutBLRepository.AddNewActivityOfUser(newWorkout);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newWorkout.WorkoutName, result.WorkoutName); // The method still returns the workout despite the exception
        }

        [Fact]
        public async Task TestDeleteAndDisplayAll_ValidActivityId()
        {
            // Arrange
            var activityId = 1;
            mockWorkoutRepo.Setup(x => x.DeleteAndDisplayAll(activityId)).ReturnsAsync(workouts);

            // Act
            var result = await workoutBLRepository.DeleteAndDisplayAll(activityId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task TestDeleteAndDisplayAll_InvalidActivityId()
        {
            // Arrange
            var activityId = 99; // Assuming this activity ID does not exist
            mockWorkoutRepo.Setup(x => x.DeleteAndDisplayAll(activityId)).ReturnsAsync(new List<Workout>());

            // Act
            var result = await workoutBLRepository.DeleteAndDisplayAll(activityId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGetWorkOutDetailsByUserID_ValidUserId()
        {
            // Arrange
            var userId = 1;
            mockWorkoutRepo.Setup(x => x.GetWorkOutDetailsByUserID(userId)).ReturnsAsync(workouts);

            // Act
            var result = await workoutBLRepository.GetWorkOutDetailsByUserID(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task TestGetWorkOutDetailsByUserID_InvalidUserId()
        {
            // Arrange
            var userId = 99; // Assuming this user ID does not exist
            mockWorkoutRepo.Setup(x => x.GetWorkOutDetailsByUserID(userId)).ReturnsAsync(new List<Workout>());

            // Act
            var result = await workoutBLRepository.GetWorkOutDetailsByUserID(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGoalReachedOrNot_ValidData()
        {
            // Arrange
            var goalName = "Morning Run";
            var activityId = 1;
            var goalDetails = fitnessGoals.First();
            var workout = workouts.First();
            mockWorkoutRepo.Setup(x => x.GoalReachedOrNot(activityId)).ReturnsAsync(workout);
            mockFitnessGoalRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync(goalDetails);

            // Act
            var result = await workoutBLRepository.GoalReachedOrNot(goalName, activityId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsAchieved);
        }

        [Fact]
        public async Task TestGoalReachedOrNot_InvalidData()
        {
            // Arrange
            var goalName = "InvalidGoal";
            var activityId = 99; // Assuming this activity ID does not exist
            mockWorkoutRepo.Setup(x => x.GoalReachedOrNot(activityId)).ReturnsAsync((Workout)null);
            mockFitnessGoalRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync((FitnessGoal)null);

            // Act
            var result = await workoutBLRepository.GoalReachedOrNot(goalName, activityId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsAchieved);
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
            mockWorkoutRepo.Setup(x => x.UpdateWorkoutByDetails(updatedWorkout)).ReturnsAsync(updatedWorkout);

            // Act
            var result = await workoutBLRepository.UpdateWorkoutByDetails(updatedWorkout);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedWorkout.WorkoutName, result.WorkoutName);
        }

        [Fact]
        public async Task TestUpdateWorkoutByDetails_InvalidData()
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
            mockWorkoutRepo.Setup(x => x.UpdateWorkoutByDetails(updatedWorkout)).ReturnsAsync((Workout)null);

            // Act
            var result = await workoutBLRepository.UpdateWorkoutByDetails(updatedWorkout);

            // Assert
            Assert.Null(result);
        }
    }
}
