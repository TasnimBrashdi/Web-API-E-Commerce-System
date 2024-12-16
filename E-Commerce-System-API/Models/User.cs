using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models
{
    public class User
    {
        [Key]
        [JsonIgnore]
        public int UId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must be in a valid format.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must be at least 8 characters long, with at least one letter and one number.")]
        public string Password { get; set; }
        [Required]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format. Must be in international format, e.g., +123456789.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
