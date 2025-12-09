using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Models
{
    public class FitnessGoal
    {
        public int FitnessGoalId { get; set; }
        public string? GoalType { get; set; }
        public string? GoalName { get; set; }
        public TimeOnly Duration { get; set; }
        [Precision(3,2)]
        public double Distance { get; set; }
        public int CaloriesBurned { get; set; }  
        
        
    }
}
