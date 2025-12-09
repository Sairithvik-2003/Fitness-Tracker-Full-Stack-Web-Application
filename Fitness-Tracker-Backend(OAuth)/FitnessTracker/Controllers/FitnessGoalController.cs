using FitnessTracker.BLRepository;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
        [HttpGet]
        public IActionResult GetAuthenticationKey()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"some_id"),

            };

            var secretBytes = System.Text.Encoding.UTF8.GetBytes(Constants.Secret);//string to binary
            var key = new SymmetricSecurityKey(secretBytes);
            var algorthim = SecurityAlgorithms.HmacSha256;
            var signingCredentials = new SigningCredentials(key, algorthim);

            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(24),
                signingCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { access_token = tokenJson });
            // return RedirectToAction("Index");
        }
        [Authorize]
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
        [Authorize]
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
