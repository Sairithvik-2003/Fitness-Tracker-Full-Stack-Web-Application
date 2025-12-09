using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrackerTests.BLTest
{
    public class FitnessBLRepositoryTest
    {
        private readonly Mock<IFitnessGoalRepository> mockRepo;
        private readonly FitnessBLRepository fitnessBLRepository;
        private readonly List<FitnessGoal> fitnessGoals;

        public FitnessBLRepositoryTest()
        {
            mockRepo = new Mock<IFitnessGoalRepository>();
            fitnessBLRepository = new FitnessBLRepository(mockRepo.Object);

            fitnessGoals = new List<FitnessGoal>
            {
                new() { FitnessGoalId = 1, GoalType = "Outdoor", GoalName = "Morning Run", Duration = new TimeOnly(1, 0, 0), Distance = 5.0, CaloriesBurned = 500 },
                new() { FitnessGoalId = 2, GoalType = "Indoor", GoalName = "Yoga", Duration = new TimeOnly(0, 45, 0), Distance = 0.0, CaloriesBurned = 200 }
            };
        }

        [Fact]
        public async Task TestGetGoals()
        {
            // Arrange
            mockRepo.Setup(x => x.GetGoals()).ReturnsAsync(fitnessGoals);

            // Act
            var result = await fitnessBLRepository.GetGoals();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task TestGetGoals_EmptyList()
        {
            // Arrange
            mockRepo.Setup(x => x.GetGoals()).ReturnsAsync(new List<FitnessGoal>());

            // Act
            var result = await fitnessBLRepository.GetGoals();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task TestGetGoalByName_ValidName()
        {
            // Arrange
            var goalName = "Morning Run";
            var expectedGoal = new FitnessGoal { FitnessGoalId = 1, GoalType = "Outdoor", GoalName = goalName, Duration = new TimeOnly(1, 0, 0), Distance = 5.0, CaloriesBurned = 500 };
            mockRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync(expectedGoal);

            // Act
            var result = await fitnessBLRepository.GetGoalByName(goalName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(goalName, result.GoalName);
        }
        [Fact]
        public async Task TestGetGoalByName_InvalidName()
        {
            // Arrange
            var goalName = "InvalidGoal";
            mockRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync((FitnessGoal)null);

            // Act
            var result = await fitnessBLRepository.GetGoalByName(goalName);

            // Assert
            Assert.Null(result); // Check if result is null before accessing its properties
        }

    }
}
