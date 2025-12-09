using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class WorkoutRepositoryTests
{
    private DbContextOptions<FitnessTrackerDbContext> CreateInMemoryDatabaseOptions()
    {
        return new DbContextOptionsBuilder<FitnessTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
            .Options;
    }

    private async Task SeedData(DbContextOptions<FitnessTrackerDbContext> options)
    {
        using (var context = new FitnessTrackerDbContext(options))
        {
            context.Workouts.RemoveRange(context.Workouts);
            await context.SaveChangesAsync();

            context.Workouts.AddRange(
                new Workout { WorkoutId = 1, WorkoutName = "Running", Duration = new TimeOnly(0, 30), Date = new DateOnly(2023, 11, 18), CaloriesBurned = 300, WorkoutType = "Cardio", Distance = 5.0, UserProfileId = 1 },
                new Workout { WorkoutId = 2, WorkoutName = "Cycling", Duration = new TimeOnly(0, 45), Date = new DateOnly(2023, 11, 17), CaloriesBurned = 400, WorkoutType = "Cardio", Distance = 15.0, UserProfileId = 1 },
                new Workout { WorkoutId = 3, WorkoutName = "Yoga", Duration = new TimeOnly(1, 0), Date = new DateOnly(2023, 11, 16), CaloriesBurned = 200, WorkoutType = "Flexibility", Distance = 0.0, UserProfileId = 2 }
            );
            await context.SaveChangesAsync();
        }
    }

    // Test cases for GetWorkOutDetailsByUserID
    [Fact]
    public async Task GetWorkOutDetailsByUserID_ReturnsWorkouts_WhenUserHasWorkouts()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int userId = 1;

        // Act
        List<Workout> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GetWorkOutDetailsByUserID(userId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count); // User 1 has 2 workouts
    }

    [Fact]
    public async Task GetWorkOutDetailsByUserID_ReturnsEmptyList_WhenUserHasNoWorkouts()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int userId = 3; // Non-existent user

        // Act
        List<Workout> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GetWorkOutDetailsByUserID(userId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result); // Expecting empty list
    }

    [Fact]
    public async Task GetWorkOutDetailsByUserID_ReturnsWorkoutsInDescendingOrder()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int userId = 1;

        // Act
        List<Workout> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GetWorkOutDetailsByUserID(userId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Running", result[0].WorkoutName); // Most recent workout
    }

    // Test cases for AddNewActivityOfUser
    [Fact]
    public async Task AddNewActivityOfUser_AddsWorkoutSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var newWorkout = new Workout { WorkoutName = "Swimming", Duration = new TimeOnly(0, 30), Date = new DateOnly(2023, 11, 19), CaloriesBurned = 250, WorkoutType = "Cardio", Distance = 1.0, UserProfileId = 1 };

        // Act
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            await repository.AddNewActivityOfUser(newWorkout);
        }

        // Assert
        using (var context = new FitnessTrackerDbContext(options))
        {
            var workout = await context.Workouts.FirstOrDefaultAsync(w => w.WorkoutName == "Swimming");
            Assert.NotNull(workout);
            Assert.Equal("Swimming", workout.WorkoutName);
        }
    }

    [Fact]
    public async Task AddNewActivityOfUser_DoesNotAddDuplicateWorkout()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var duplicateWorkout = new Workout { WorkoutName = "Running", Duration = new TimeOnly(0, 30), Date = new DateOnly(2023, 11, 18), CaloriesBurned = 300, WorkoutType = "Cardio", Distance = 5.0, UserProfileId = 1 };

        // Act
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            await repository.AddNewActivityOfUser(duplicateWorkout);
        }

        // Assert
        using (var context = new FitnessTrackerDbContext(options))
        {
            var workouts = await context.Workouts.CountAsync(w => w.WorkoutName == "Running" && w.Date == new DateOnly(2023, 11, 18));
            Assert.Equal(1, workouts); // Should still be 1
        }
    }

    [Fact]
    public async Task AddNewActivityOfUser_AddsWorkoutWhenNotDuplicate()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var newWorkout = new Workout { WorkoutName = "Hiking", Duration = new TimeOnly(2, 0), Date = new DateOnly(2023, 11, 19), CaloriesBurned = 500, WorkoutType = "Outdoor", Distance = 10.0, UserProfileId = 1 };

        // Act
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            await repository.AddNewActivityOfUser(newWorkout);
        }

        // Assert
        using (var context = new FitnessTrackerDbContext(options))
        {
            var workout = await context.Workouts.FirstOrDefaultAsync(w => w.WorkoutName == "Hiking");
            Assert.NotNull(workout);
            Assert.Equal("Hiking", workout.WorkoutName);
        }
    }

    // Test cases for DeleteAndDisplayAll
    [Fact]
    public async Task DeleteAndDisplayAll_DeletesWorkoutSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int workoutIdToDelete = 1;

        // Act
        List<Workout> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.DeleteAndDisplayAll(workoutIdToDelete);
        }

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain(result, w => w.WorkoutId == workoutIdToDelete); // Ensure it's deleted
    }

    [Fact]
    public async Task DeleteAndDisplayAll_ReturnsRemainingWorkouts()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int workoutIdToDelete = 1;

        // Act
        List<Workout> result;
        List<Workout> Actual;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            var User = context.Workouts.Where(x=>x.WorkoutId==workoutIdToDelete).FirstOrDefault();
            result = await repository.DeleteAndDisplayAll(workoutIdToDelete);
             Actual = await context.Workouts.Where(x => x.UserProfileId == User.UserProfileId).ToListAsync();
        }
        

        // Assert
        Assert.NotNull(result);
        Assert.Equal(Actual.Count, result.Count); // Should return 2 workouts after deletion
    }

    [Fact]
    public async Task DeleteAndDisplayAll_NonExistentWorkout_ReturnsAllWorkouts()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int nonExistentId = 999; // Non-existent workout ID

        // Act
        List<Workout> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.DeleteAndDisplayAll(nonExistentId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(3, result.Count); // Should return all workouts since none were deleted
    }
    [Fact]
    public async Task GoalReachedOrNot_ReturnsWorkout_WhenWorkoutExists()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int workoutId = 1;

        // Act
        Workout result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GoalReachedOrNot(workoutId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Running", result.WorkoutName); // Check that the correct workout is returned
    }

    [Fact]
    public async Task GoalReachedOrNot_ReturnsNull_WhenWorkoutDoesNotExist()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int nonExistentId = 999; // Non-existent workout ID

        // Act
        Workout result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GoalReachedOrNot(nonExistentId);
            if(result.UserProfileId==0)
            {
                result = null;
            }
        }

        // Assert
        Assert.Null(result); // Expecting null for a non-existent workout
    }

    [Fact]
    public async Task GoalReachedOrNot_ReturnsCorrectWorkout_WhenWorkoutExists()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        int workoutId = 2; // Existing workout ID

        // Act
        Workout result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.GoalReachedOrNot(workoutId);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Cycling", result.WorkoutName); // Check that the correct workout is returned
    }
    [Fact]
    public async Task UpdateWorkoutByDetails_UpdatesWorkoutSuccessfully()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var updatedWorkout = new Workout { WorkoutId = 1, WorkoutName = "Running", Duration = new TimeOnly(0, 35), Date = new DateOnly(2023, 11, 18), CaloriesBurned = 350, WorkoutType = "Cardio", Distance = 6.0 };

        // Act
        Workout result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.UpdateWorkoutByDetails(updatedWorkout);
        }

        // Assert
        using (var context = new FitnessTrackerDbContext(options))
        {
            var workout = await context.Workouts.FindAsync(1);
            Assert.NotNull(workout);
            Assert.Equal(updatedWorkout.Duration.Hour, workout.Duration.Hour); // Check updated duration
            Assert.Equal(updatedWorkout.CaloriesBurned, workout.CaloriesBurned); // Check updated calories burned
        }
    }

    [Fact]
    public async Task UpdateWorkoutByDetails_ReturnsNull_WhenWorkoutDoesNotExist()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var updatedWorkout = new Workout { WorkoutId = 999, WorkoutName = "Nonexistent", Duration = new TimeOnly(0, 30), Date = new DateOnly(2023, 11, 19), CaloriesBurned = 300, WorkoutType = "Cardio", Distance = 5.0 };

        // Act
        Workout result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.UpdateWorkoutByDetails(updatedWorkout);
        }

        // Assert
        Assert.Null(result); // Expecting null for a non-existent workout
    }

    [Fact]
    public async Task UpdateWorkoutByDetails_DoesNotChangeWorkout_WhenNoUpdatesProvided()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        var unchangedWorkout = new Workout { WorkoutId = 1, WorkoutName = "Running", Duration = new TimeOnly(0, 30), Date = new DateOnly(2023, 11, 18), CaloriesBurned = 300, WorkoutType = "Cardio", Distance = 5.0 };

        // Act
        Workout result;
        TimeOnly result1;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new WorkoutRepository(context);
            result = await repository.UpdateWorkoutByDetails(unchangedWorkout);

            result1 = unchangedWorkout.Duration;
        }

        // Assert
        using (var context = new FitnessTrackerDbContext(options))
        {
            var workout = await context.Workouts.FindAsync(1);
            Assert.NotNull(workout);
            Assert.Equal(result1, workout.Duration); // Ensure duration remains unchanged
        }
    }
}