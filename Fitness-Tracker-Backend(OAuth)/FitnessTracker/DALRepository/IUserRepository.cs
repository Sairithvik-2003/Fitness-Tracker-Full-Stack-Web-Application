using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessTracker.DALRepository
{
    public interface IUserRepository
    {
        public Task<UserProfile> AddNewUser(UserProfile User);
        public Task<Login> GetUserDetailsByAuthentication(string Email, string password);
        public Task<UserDetails> GetUserProfileByID(int id);
        public Task<bool> DeleteUser(int UserId);
    }
}
