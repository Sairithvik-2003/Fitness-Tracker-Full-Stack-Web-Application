using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DALRepository
{
    public class FitnessGoalRepository:IFitnessGoalRepository
    {
        private readonly FitnessTrackerDbContext _context;
        public List<UserProfile> UserProfiles = new();
        public List<FitnessGoal> FitnessGoals = new();
        public FitnessGoalRepository(FitnessTrackerDbContext db)
        {
            _context = db;
        }

        public async Task<List<FitnessGoal>> GetGoals()
        {
            var Goal = new List<FitnessGoal>();
            try
            {
                var UserGoal = await _context.FitnessGoals.ToListAsync();
                if(UserGoal != null) 
                {
                    Goal = UserGoal;
                    
                }
                return Goal;
            }
            catch (Exception ) 
            {
                return Goal;
            }
            
        }
        public async Task<FitnessGoal> GetGoalByName(string Goalname)
        {
            var Goal = new FitnessGoal();
            try
            {
                var UserGoal = await _context.FitnessGoals.Where(x => x.GoalName == Goalname).FirstOrDefaultAsync();
                if (UserGoal != null)
                {
                    Goal = UserGoal;
                    
                }
                return Goal;
            }
            catch (Exception)
            {
                return Goal;
            }

        }
    }
}
