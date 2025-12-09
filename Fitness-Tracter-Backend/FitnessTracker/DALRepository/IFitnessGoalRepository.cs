using FitnessTracker.Models;

namespace FitnessTracker.DALRepository
{
    public interface IFitnessGoalRepository
    {
        public Task<List<FitnessGoal>> GetGoals();
        public Task<FitnessGoal> GetGoalByName( string Goalname);
    }
}
