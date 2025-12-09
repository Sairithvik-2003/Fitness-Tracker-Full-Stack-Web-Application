using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DALRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly FitnessTrackerDbContext _context;
        public List<UserProfile> UserProfiles = new();
        public UserRepository(FitnessTrackerDbContext db)
        {
            _context = db;
        }
        /* public async Task<UserProfile> AddNewUser(UserProfile User)
         {
             try
             {
                 _context.UserProfiles.Add(User);
                 await _context.SaveChangesAsync();

                 var id = User.Id;
                 return User;
             }
             catch (Exception)
             {
                 return User;
             }
         }*/
        public async Task<UserProfile> AddNewUser(UserProfile User)
        {
            bool flag = false;
            try
            {
                var EmailList = await _context.UserProfiles.Select(UserProfile => UserProfile.Email).ToListAsync();
                foreach (var Email in EmailList) 
                {
                    if(Email==User.Email)
                    {
                        flag = true;
                    }
                }
                if (flag == false)
                {
                    _context.UserProfiles.Add(User);
                    await _context.SaveChangesAsync();

                   
                }
               /* var id = User.Id;*/
              /* if (id != 0) {*/ return User; /*}*/
               /* return null;*/
            }
            catch (Exception)
            {
                return User;
            }
        }

        public async Task<Login> GetUserDetailsByAuthentication(string Email, string password)
        {
            var User = new Login();
            try
            {
                var userData = await _context.UserProfiles
                                  .Where(u => u.Email == Email && u.Password == password)
                                    .Select(u => new
                                    {
                                        id = u.Id,
                                        Name = u.FullName,
                                        eMail = u.Email,

                                    }).FirstOrDefaultAsync();
                if (userData != null)
                {
                    User.UserId = userData.id;
                    User.Email = userData.eMail;
                    User.Fullname = userData.Name;
                    return User ;
                }

                return null;
            }
            catch (Exception)
            {
                return User;
            }

        }
        public async Task<UserDetails> GetUserProfileByID(int id)
        {
            var UserDetails = new UserDetails();
            try
            {
                var UserDetail = await _context.UserProfiles.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (UserDetail != null)
                {
                    UserDetails.Id = UserDetail.Id;
                    UserDetails.FullName = UserDetail.FullName;
                    UserDetails.Email = UserDetail.Email;
                    UserDetails.Gender = UserDetail.Gender;
                    UserDetails.Height = UserDetail.Height;
                    UserDetails.Weight = UserDetail.Weight;
                    UserDetails.DateOfBirth = UserDetail.DateOfBirth;
                }
                return UserDetails;
            }
            catch (Exception)
            {
                return UserDetails;
            }
        }
        public async Task<bool> DeleteUser(int UserId)
        {
            try
            {
                var user = await _context.UserProfiles.FindAsync(UserId);
                if (user == null)
                {
                    return false;
                }
                _context.UserProfiles.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
