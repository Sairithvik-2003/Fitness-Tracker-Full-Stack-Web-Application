using FitnessTracker.Models;

namespace FitnessTracker.BLRepository
{
    public interface IUserBLRepository
    {
        public Task<UserProfile> AddNewUser(UserProfile User);
        public Task<Login> GetUserDetailsByAuthentication(string Email, string password);
        public Task<UserDetails> GetUserProfileByID(int id);
        public Task<bool> DeleteUser(int UserId);
    }
}
