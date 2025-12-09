using FitnessTracker.Models;

namespace FitnessTracker.BLRepository
{
    public interface IWorkoutBLRepository
    {
        public Task<List<Workout>> GetWorkOutDetailsByUserID(int id);
        public Task<Workout> AddNewActivityOfUser(Workout Activity);
        public Task<List<Workout>> DeleteAndDisplayAll(int activityId);
        public Task<WorkoutandFitnessGoal> GoalReachedOrNot( string Goalname,int activityId);
        public Task<Workout> UpdateWorkoutByDetails(Workout workouts);
    }
}
