using FitnessTracker.BLRepository;
using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBLRepository _userBLRepository;

        public UserController(IUserBLRepository userBLRepository)
        {
            _userBLRepository = userBLRepository;
        }
        /*[HttpPost(Name = "SignUp/Register")]
        public async Task<ActionResult<UserProfile>> AddNewUser(string name, string email, string pass, string gender, double weight, double height, string date)
        {
            var newUser = new UserProfile();
            try
            {
                DateOnly newdate = DateOnly.Parse(date);

                newUser.FullName = name;
                newUser.Email = email;
                newUser.Gender = gender;
                newUser.Weight = weight;
                newUser.Height = height;
                newUser.Password = pass;
                newUser.DateOfBirth = newdate;

                if (!ModelState.IsValid)
                    return BadRequest(newUser);
                await _userBLRepository.AddNewUser(newUser);

                return Ok(newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }*/
        [HttpPost(Name = "SignUp/Register")]
        public async Task<ActionResult<UserProfile>> AddNewUser(string name, string email, string pass, string gender, double weight, double height, string date)
        {
            var newUser = new UserProfile();
            var UpdatedUser = new UserProfile();

            try
            {
                DateOnly newdate = DateOnly.Parse(date);

                newUser.FullName = name;
                newUser.Email = email;
                newUser.Gender = gender;
                newUser.Weight = weight;
                newUser.Height = height;
                newUser.Password = pass;
                newUser.DateOfBirth = newdate;

                if (newUser.Email.Length == 0 || newUser.Password.Length == 0)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(newUser);
                UpdatedUser = await _userBLRepository.AddNewUser(newUser);
               
                if (UpdatedUser == null)
                {
                    return BadRequest("Email already Exists");
                }
                return Ok(UpdatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
        [HttpGet(Name = "UserLogin")]
        public async Task<ActionResult<Login>> GetUserDetailsByAuthentication(string Email, string password)
        {
            var UserDetails = new Login();
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(UserDetails);
                UserDetails = await _userBLRepository.GetUserDetailsByAuthentication(Email, password);
                if (UserDetails == null)
                {
                    return NotFound();
                }
                return Ok(UserDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }

        }
        [HttpGet(Name = "GetDetailsById")]
        public async Task<ActionResult<UserDetails>> GetUserProfileByID(int id)
        {
            var UserData = new UserDetails();
            try
            {
                if (!ModelState.IsValid)
                    BadRequest(UserData);
                UserData = await _userBLRepository.GetUserProfileByID(id);
                if (UserData == null)
                {
                    return NotFound();
                }
                return UserData;
            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
        [HttpDelete(Name = "DeleteUser")]

        public async Task<ActionResult<bool>> DeleteUser(int UserId)
        {
            bool Status=false;  
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(Status);
                Status = await _userBLRepository.DeleteUser(UserId);
                return Ok(Status);

            }
            catch (Exception ex)
            {
                return StatusCode(404, ex.Message);
            }
        }
    }
}
