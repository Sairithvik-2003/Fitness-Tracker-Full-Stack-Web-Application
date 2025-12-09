using FitnessTracker.DALRepository;
using FitnessTracker.Models;

namespace FitnessTracker.BLRepository
{
    public class UserBLRepository:IUserBLRepository
    {
        private readonly IUserRepository _userRepository;
        public List<UserProfile> UserProfiles = new();
        public UserBLRepository(IUserRepository db)
        {
            _userRepository = db;
        }

        public async Task<UserProfile> AddNewUser(UserProfile User)
        {
            var UserDetails = new UserProfile();
            try
            {
                UserDetails=await _userRepository.AddNewUser(User);
                if(UserDetails.Id == 0)
                {
                    return null;
                }
                return UserDetails;
            }
            catch (Exception)
            {
                return User;
            }
        }

        public async Task<Login> GetUserDetailsByAuthentication(string Email, string password)
        {
            var UserData = new Login();
            try
            {
                UserData = await _userRepository.GetUserDetailsByAuthentication(Email, password);
                return UserData;
            }
            catch (Exception) 
            {
                return UserData;
            }
        }
        public async Task<UserDetails> GetUserProfileByID(int id)
        {
            var UserData = new UserDetails();
            try
            {
                UserData = await _userRepository.GetUserProfileByID(id);
                return UserData;
            }
            catch(Exception) 
            {
                return UserData;
            }
        }
        public async Task<bool> DeleteUser(int UserId)
        {
            bool status = false;
            try
            {
                status = await _userRepository.DeleteUser(UserId);  
                return status;
            }
            catch (Exception)
            {

                return status;
            }
        }

    }
}
