using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class UserDetails
    {
        public int Id { get; set; }

        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public string? Gender { get; set; }
        [Precision(3, 2)]
        public double Weight { get; set; }
        [Precision(3, 2)]
        public double Height { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
