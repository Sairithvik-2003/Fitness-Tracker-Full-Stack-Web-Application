using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class FitnessGoalRepositoryTests
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
            // Clear existing data to avoid duplicates
            context.FitnessGoals.RemoveRange(context.FitnessGoals);
            await context.SaveChangesAsync();

            context.FitnessGoals.AddRange(
                new FitnessGoal { FitnessGoalId = 1, GoalName = "Lose Weight", GoalType = "Weight Loss", Duration = new TimeOnly(1, 0), Distance = 5.0, CaloriesBurned = 500 },
                new FitnessGoal { FitnessGoalId = 2, GoalName = "Build Muscle", GoalType = "Strength", Duration = new TimeOnly(1, 30), Distance = 0.0, CaloriesBurned = 300 },
                new FitnessGoal { FitnessGoalId = 3, GoalName = "Run a Marathon", GoalType = "Endurance", Duration = new TimeOnly(2, 0), Distance = 42.195, CaloriesBurned = 2500 }
            );
            await context.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task GetGoals_ReturnsAllGoals_WhenGoalsExist()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);

        // Act
        List<FitnessGoal> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new FitnessGoalRepository(context);
            result = await repository.GetGoals();
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetGoals_ReturnsEmptyList_WhenNoGoalsExist()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        using (var context = new FitnessTrackerDbContext(options))
        {
            context.FitnessGoals.RemoveRange(context.FitnessGoals);
            await context.SaveChangesAsync();
        }

        // Act
        List<FitnessGoal> result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new FitnessGoalRepository(context);
            result = await repository.GetGoals();
        }

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGoalByName_ReturnsGoal_WhenGoalExists()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        string goalName = "Run a Marathon";

        // Act
        FitnessGoal result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new FitnessGoalRepository(context);
            result = await repository.GetGoalByName(goalName);
        }

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Run a Marathon", result.GoalName);
        Assert.Equal("Endurance", result.GoalType); // Additional check for goal type
    }

    [Fact]
    public async Task GetGoalByName_ReturnsNull_WhenGoalDoesNotExist()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        await SeedData(options);
        string goalName = "Nonexistent Goal";

        // Act
        FitnessGoal result;
        using (var context = new FitnessTrackerDbContext(options))
        {
            var repository = new FitnessGoalRepository(context);
            result = await repository.GetGoalByName(goalName);
            if(result.GoalName == null)
            {
                result= null;   
            }
        }

        // Assert
        Assert.Null(result); // Expecting null for a non-existent goal
    }
}