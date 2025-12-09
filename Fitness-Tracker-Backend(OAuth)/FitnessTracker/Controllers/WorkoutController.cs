using FitnessTracker.BLRepository;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutBLRepository _workoutBLRepository;
        public WorkoutController(IWorkoutBLRepository workoutBLRepository)
        {
            _workoutBLRepository = workoutBLRepository;
        }

        [HttpGet("UserHistoryOfActivities")]
        public async Task<ActionResult<List<Workout>>> GetWorkOutDetailsByUserID(int userId)
        {
            var WorkoutHistory = new List<Workout>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(WorkoutHistory);
                WorkoutHistory = await _workoutBLRepository.GetWorkOutDetailsByUserID(userId);
                if (WorkoutHistory.Count == 0)
                {
                    return Ok(WorkoutHistory);
                }
                else if (WorkoutHistory == null)
                {
                    return NotFound();
                }
                return Ok(WorkoutHistory);

            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        [HttpPost(Name = "AddWorkoutDetails")]
        public async Task<ActionResult<Workout>> AddNewActivityOfUser(string WorkoutType, string WorkoutName, string Duration, double distance, int caloriesBurned,string Date,int UserId)
        {
            var NewWorkout = new Workout();
            
            try
            {
                DateOnly newdate = DateOnly.Parse(Date);
                /*  TimeOnly NewTime =TimeSpan.TimeOnly.Parse(Duration);*/
                TimeOnly time = TimeOnly.Parse(Duration);

                NewWorkout.WorkoutType = WorkoutType;
                NewWorkout.WorkoutName = WorkoutName;
                NewWorkout.Duration = time;
                NewWorkout.Distance = distance;
                NewWorkout.CaloriesBurned = caloriesBurned;
                NewWorkout.Date = newdate;
                NewWorkout.UserProfileId = UserId;
               
               
                if (!ModelState.IsValid)
                    return BadRequest(NewWorkout);
                await _workoutBLRepository.AddNewActivityOfUser(NewWorkout);
                
                return Ok(NewWorkout);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        [HttpDelete]
        public async Task<ActionResult<List<Workout>>> DeleteAndDisplayAll(int activityId)
        {
            var RemainingWorkouts = new List<Workout>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(RemainingWorkouts);
                RemainingWorkouts = await _workoutBLRepository.DeleteAndDisplayAll(activityId);
                return Ok(RemainingWorkouts);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        [HttpGet(Name = "GoalReachedOrNot")]
        public async Task<ActionResult<WorkoutandFitnessGoal>> GoalReachedOrNot(string Goalname, int activityId)
        {
            var ComparisionOfWorkouts = new WorkoutandFitnessGoal();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ComparisionOfWorkouts);
                ComparisionOfWorkouts = await _workoutBLRepository.GoalReachedOrNot(Goalname, activityId);
                return Ok(ComparisionOfWorkouts);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        [HttpPut(Name ="UpdateWorkouts")]
        public async Task<ActionResult<Workout>> UpdateWorkoutByDetails(string WorkoutType, string WorkoutName, string Duration, double distance, int caloriesBurned, string Date, int UserId)
        {

            var NewWorkout = new Workout();
            
            try
            {
                DateOnly newdate = DateOnly.Parse(Date);
                /*  TimeOnly NewTime =TimeSpan.TimeOnly.Parse(Duration);*/
                TimeOnly time = TimeOnly.Parse(Duration);
                /*if (string.IsNullOrWhiteSpace(WorkoutName)) { return BadRequest("WorkoutName is required"); }*/

                NewWorkout.WorkoutType = WorkoutType;
                NewWorkout.WorkoutName = WorkoutName;
                NewWorkout.Duration = time;
                NewWorkout.Distance = distance;
                NewWorkout.CaloriesBurned = caloriesBurned;
                NewWorkout.Date = newdate;
                NewWorkout.UserProfileId = UserId;
                if (!ModelState.IsValid)
                    BadRequest(NewWorkout);
                 await _workoutBLRepository.UpdateWorkoutByDetails(NewWorkout);
                return Ok(NewWorkout);
            }
            catch (Exception ex) 
            {
                return StatusCode(400,ex.Message);
            }
        }
    }
}
