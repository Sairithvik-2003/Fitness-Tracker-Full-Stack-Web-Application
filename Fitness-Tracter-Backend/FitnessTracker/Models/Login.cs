using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class Login
    {
        public int UserId {  get; set; } 
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
      /*  [Required]
        [DataType(DataType.Password)]*/
        public string? Fullname { get; set; }
    }
}
