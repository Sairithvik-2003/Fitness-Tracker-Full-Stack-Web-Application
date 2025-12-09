using FitnessTracker.Models;

namespace FitnessTracker.BLRepository
{
    public interface IFitnessGoalBLRepository
    {
        public Task<List<FitnessGoal>> GetGoals();
        public Task<FitnessGoal> GetGoalByName( string Goalname);
    }
}
