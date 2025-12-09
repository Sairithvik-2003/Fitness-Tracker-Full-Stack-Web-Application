using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.BLRepository
{
    public class WorkoutBLRepository:IWorkoutBLRepository
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly IFitnessGoalBLRepository _fitnessRepository;
        public List<Workout> Workouts = new();
        public List<FitnessGoal> Fitnessess = new();
        public WorkoutBLRepository(IWorkoutRepository db , IFitnessGoalBLRepository fitnessRepository)
        {
            _workoutRepository = db;
            _fitnessRepository = fitnessRepository;
        }

        public async Task<Workout> AddNewActivityOfUser(Workout Activity)
        {
            try
            {
                await _workoutRepository.AddNewActivityOfUser(Activity);
                return Activity;
            }
            catch (Exception)
            {
                return Activity;
            }

        }

        public async Task<List<Workout>> DeleteAndDisplayAll(int activityId)
        {
            var RemainingWorkouts = new List<Workout>();
            try
            {
                RemainingWorkouts = await _workoutRepository.DeleteAndDisplayAll(activityId);
                return RemainingWorkouts;
            }
            catch (Exception) 
            {
                return RemainingWorkouts;
            }
        }

        public async Task<List<Workout>> GetWorkOutDetailsByUserID(int id)
        {
            var UserAllWorkouts = new List<Workout>();
            try
            {
                UserAllWorkouts = await _workoutRepository.GetWorkOutDetailsByUserID(id);
                return UserAllWorkouts;
            }
            catch (Exception) 
            {
                return UserAllWorkouts;
            }
        }

        public async Task<WorkoutandFitnessGoal> GoalReachedOrNot( string Goalname,int activityId )
        {
            var HasReachedGoal = new WorkoutandFitnessGoal();
            try
            {
                var Workouts = await _workoutRepository.GoalReachedOrNot(activityId);
                var FitnessGoal = await _fitnessRepository.GetGoalByName(Goalname);
                if(Workouts.WorkoutName == FitnessGoal.GoalName && Workouts.CaloriesBurned > FitnessGoal.CaloriesBurned && Workouts.Duration > FitnessGoal.Duration && Workouts.Distance>FitnessGoal.Distance)
                {
                    HasReachedGoal.IsAchieved = true; //Default it is false.
                }

                if (Workouts.WorkoutName == FitnessGoal.GoalName)
                {
                   
                    HasReachedGoal.ActivityType = FitnessGoal.GoalType;
                    HasReachedGoal.ActivityName = Workouts.WorkoutName;
                    TimeSpan goalDuration = FitnessGoal.Duration.ToTimeSpan();
                    TimeSpan workoutDuration = Workouts.Duration.ToTimeSpan();
                    TimeSpan remainingDuration = goalDuration > workoutDuration ? goalDuration - workoutDuration : workoutDuration-goalDuration;
                    HasReachedGoal.Duration = TimeOnly.FromTimeSpan(remainingDuration);
                    HasReachedGoal.Date = Workouts.Date;
                    HasReachedGoal.Distance = Math.Abs(FitnessGoal.Distance - Workouts.Distance);
                    HasReachedGoal.CaloriesBurned = Math.Abs(FitnessGoal.CaloriesBurned - Workouts.CaloriesBurned);
                   

                }
                return HasReachedGoal;
            }
            catch(Exception) 
            {
                return HasReachedGoal;
            }
           
        }
        public async Task<Workout> UpdateWorkoutByDetails(Workout workouts)
        {
            var WorkoutDetails = new Workout();
            try
            {
                WorkoutDetails = await _workoutRepository.UpdateWorkoutByDetails(workouts);
                return WorkoutDetails;
            }
            catch (Exception)
            {
                return WorkoutDetails;
            }
        }
    }
}
