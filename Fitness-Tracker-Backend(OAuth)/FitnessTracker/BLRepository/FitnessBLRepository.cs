using FitnessTracker.DALRepository;
using FitnessTracker.Models;
using System.Net;

namespace FitnessTracker.BLRepository
{
    public class FitnessBLRepository : IFitnessGoalBLRepository
    {
        private readonly IFitnessGoalRepository _FitnessGoalRepository;
        public List<UserProfile> UserProfiles = new();
        public List<FitnessGoal> FitnessGoals = new();

        public FitnessBLRepository(IFitnessGoalRepository fitnessGoalRepository)
        {
            _FitnessGoalRepository= fitnessGoalRepository;
        }
        public async Task<List<FitnessGoal>> GetGoals()
        {
            var Goals = new List<FitnessGoal>();
           try
            {
                Goals = await _FitnessGoalRepository.GetGoals();

                return Goals;
            }
            catch (Exception) 
            {
                return Goals;
            }
        }

        public async Task<FitnessGoal> GetGoalByName(string Goalname)
        {
           var UserGoal = new FitnessGoal();
            try
            {
                UserGoal = await _FitnessGoalRepository.GetGoalByName( Goalname);
                return UserGoal;
            }
            catch(Exception) 
            {
                return UserGoal;
            }
        }
    }
}
