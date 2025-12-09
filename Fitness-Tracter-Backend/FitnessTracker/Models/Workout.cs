using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Models
{
    public class Workout
    {
        public int WorkoutId { get; set; }
        public string? WorkoutType { get; set; }
        public string? WorkoutName { get; set; }
        public TimeOnly Duration { get; set; }
        [Precision(3, 2)]
        public double Distance {  get; set; }
        public int CaloriesBurned { get; set; }
        public DateOnly Date { get; set; }

        public int UserProfileId { get; set; }
    }
}
