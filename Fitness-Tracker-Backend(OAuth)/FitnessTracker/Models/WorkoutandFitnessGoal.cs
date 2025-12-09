using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Models
{
    public class WorkoutandFitnessGoal
    {
       /* public int UserId { get; set; }*/
        public string? ActivityType { get; set; }
        public string? ActivityName { get; set; }
        public TimeOnly Duration { get; set; }
        [Precision(3, 2)]
        public double Distance { get; set; }
        public int CaloriesBurned { get; set; }
        public DateOnly Date { get; set; }
        public bool IsAchieved { get; set; } = false;
    }
}
