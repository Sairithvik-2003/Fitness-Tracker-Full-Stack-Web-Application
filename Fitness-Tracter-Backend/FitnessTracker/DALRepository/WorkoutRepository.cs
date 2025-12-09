using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace FitnessTracker.DALRepository
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private readonly FitnessTrackerDbContext _context;
        /* public List<UserProfile> UserProfiles = new();
         public List<FitnessGoal> FitnessGoals = new();*/
        public List<Workout> Workouts = new();
        public WorkoutRepository(FitnessTrackerDbContext db)
        {
            _context = db;
        }

        public async Task<List<Workout>> GetWorkOutDetailsByUserID(int id)
        {
            var workouts = new List<Workout>();
            try
            {
                var WorkOutDetail = await _context.Workouts.Where(x => x.UserProfileId == id).OrderByDescending(x => x.Date).ToListAsync();

                foreach (var Activity in WorkOutDetail)
                {
                    var workout = new Workout();
                    workout.WorkoutId = Activity.WorkoutId;
                    workout.WorkoutName = Activity.WorkoutName;
                    workout.Duration = Activity.Duration;
                    workout.Date = Activity.Date;
                    workout.CaloriesBurned = Activity.CaloriesBurned;
                    workout.WorkoutType = Activity.WorkoutType;
                    workout.Distance = Activity.Distance;
                    workout.UserProfileId = Activity.UserProfileId;
                    workouts.Add(workout);
                }
                return workouts;
            }
            catch (Exception)
            {
                return workouts;
            }

        }
        public async Task<Workout> AddNewActivityOfUser(Workout Activity)
        {
            bool flag = false;
            try
            {
                var ActivityList = _context.Workouts.Where(x => x.UserProfileId == Activity.UserProfileId).ToList();
                foreach (var activity in ActivityList)
                {
                    if (Activity.WorkoutName == activity.WorkoutName && activity.Date == Activity.Date)
                    {
                        flag = true;
                        break;
                    }
                }
                if(flag == false)
                {
                    _context.Workouts.Add(Activity);
                    await _context.SaveChangesAsync();
                }
                var id = Activity.WorkoutId;
                return Activity;
            }
            catch (Exception)
            {
                return Activity;
            }
        }
        public async Task<List<Workout>> DeleteAndDisplayAll(int activityId)
        {
            var workouts = new List<Workout>();
            try
            {
                var Activity = await _context.Workouts.FindAsync(activityId);

                
                if (Activity != null)
                {
                    var UserId = Activity.UserProfileId;
                    _context.Workouts.Remove(Activity);
                    await _context.SaveChangesAsync();
                    workouts = await GetWorkOutDetailsByUserID(UserId);
                }
                return workouts;
            }
            catch (Exception)
            {
                return workouts;
            }
        }
        public async Task<Workout> GoalReachedOrNot(int activityId)
        {
           var WorkoutInstance =new Workout();
            try
            {
                var Workout = await _context.Workouts.Where(x => x.WorkoutId == activityId).FirstOrDefaultAsync();
                if (Workout != null)
                {
                    WorkoutInstance= Workout;
                }
                return WorkoutInstance;
            }
            catch (Exception)
            {
                return WorkoutInstance;
            }
        }
        public async Task<Workout> UpdateWorkoutByDetails(Workout workout)
        {
            var WorkoutDetails = _context.Workouts.FirstOrDefault(x => x.Date == workout.Date && x.WorkoutName==workout.WorkoutName);
            try
            {
                
                if (WorkoutDetails != null)
                {
                    WorkoutDetails.Duration = workout.Duration;
                    WorkoutDetails.Distance = workout.Distance;
                    WorkoutDetails.CaloriesBurned = workout.CaloriesBurned;
                    _context.Update(WorkoutDetails);
                    await _context.SaveChangesAsync();
                }
                return WorkoutDetails;

            }
            catch (Exception)
            {

                return WorkoutDetails;
            }
        }


    }
}
