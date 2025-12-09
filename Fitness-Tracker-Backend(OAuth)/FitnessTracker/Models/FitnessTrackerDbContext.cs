using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FitnessTracker.Models
{
    public class FitnessTrackerDbContext:DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<FitnessGoal> FitnessGoals { get; set; }
        public FitnessTrackerDbContext(DbContextOptions<FitnessTrackerDbContext> options) : base(options)
        {

        }
    }
}
