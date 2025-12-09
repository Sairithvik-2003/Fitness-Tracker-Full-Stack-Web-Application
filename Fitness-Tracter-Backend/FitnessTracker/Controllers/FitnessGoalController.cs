using FitnessTracker.BLRepository;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FitnessGoalController : ControllerBase
    {
        private readonly IFitnessGoalBLRepository _fitnessgoalBLRepository;
        
        public FitnessGoalController(IFitnessGoalBLRepository fitnessgoalBLRepository)
        {
            _fitnessgoalBLRepository = fitnessgoalBLRepository;
        }

        [HttpGet(Name ="GetAllGoals")]

        public async Task<ActionResult<List<FitnessGoal>>> GetGoals()
        {
            var fitnessGoals = new List<FitnessGoal>();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(fitnessGoals);
                fitnessGoals =await _fitnessgoalBLRepository.GetGoals();
                return Ok(fitnessGoals);
            }
            catch (Exception ex) 
            {
                return StatusCode(404, ex.Message);
            }
        }

        [HttpGet(Name ="GetGoalByName")]
        public async Task<ActionResult<FitnessGoal>> GetGoalByName(string GoalName)
        {
            var getGoal = new FitnessGoal();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(getGoal);
                getGoal = await _fitnessgoalBLRepository.GetGoalByName(GoalName);
                return Ok(getGoal);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}
