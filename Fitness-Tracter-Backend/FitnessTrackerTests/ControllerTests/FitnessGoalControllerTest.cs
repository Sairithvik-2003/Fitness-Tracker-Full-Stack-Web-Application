using FitnessTracker.BLRepository;
using FitnessTracker.Controllers;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FitnessTrackerTest.ControllerTest
{
    public class FitnessGoalControllerTest
    {
        private readonly Mock<IFitnessGoalBLRepository> mockRepo;
        private readonly List<FitnessGoal> fitnessGoals;

        public FitnessGoalControllerTest()
        {
            mockRepo = new Mock<IFitnessGoalBLRepository>();

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
            mockRepo.Setup(x => x.GetGoals()).ReturnsAsync(GetMockGoals());

            // Act
            var controller = new FitnessGoalController(mockRepo.Object);
            var result = await controller.GetGoals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<FitnessGoal>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task TestGetGoals_EmptyList()
        {
            // Arrange
            mockRepo.Setup(x => x.GetGoals()).ReturnsAsync(new List<FitnessGoal>());

            // Act
            var controller = new FitnessGoalController(mockRepo.Object);
            var result = await controller.GetGoals();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<FitnessGoal>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        private List<FitnessGoal> GetMockGoals()
        {
            return fitnessGoals;
        }

        [Fact]
        public async Task TestGetGoalForUser()
        {
            // Arrange
            var goalName = "Morning Run";
            mockRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync(GetMockGoalForUser(goalName));

            // Act
            var controller = new FitnessGoalController(mockRepo.Object);
            var result = await controller.GetGoalByName(goalName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<FitnessGoal>(okResult.Value);
            Assert.Equal(goalName, returnValue.GoalName);
        }

        [Fact]
        public async Task TestGetGoalForUser_InvalidName()
        {
            // Arrange
            var goalName = "InvalidName";
            mockRepo.Setup(x => x.GetGoalByName(goalName)).ReturnsAsync(GetMockGoalForUser(goalName));

            // Act
            var controller = new FitnessGoalController(mockRepo.Object);
            var result = await controller.GetGoalByName(goalName);

            // Assert
            var noContentResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<FitnessGoal>(noContentResult.Value);
            Assert.Null(returnValue.GoalName);
        }

        private FitnessGoal GetMockGoalForUser(string goalName)
        {
            return fitnessGoals.FirstOrDefault(x => x.GoalName == goalName) ?? new FitnessGoal();
        }
    }
}
