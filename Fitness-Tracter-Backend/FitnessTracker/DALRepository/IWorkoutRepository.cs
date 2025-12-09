using FitnessTracker.Models;

namespace FitnessTracker.DALRepository
{
    public interface IWorkoutRepository
    {
        public Task<List<Workout>> GetWorkOutDetailsByUserID(int id);
        public Task<Workout> AddNewActivityOfUser(Workout Activity);
        public Task<List<Workout>> DeleteAndDisplayAll(int activityId);
        public Task<Workout> GoalReachedOrNot(int  activityId );
        public Task<Workout> UpdateWorkoutByDetails(Workout workouts);

    }
}
