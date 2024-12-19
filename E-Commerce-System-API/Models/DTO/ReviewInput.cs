using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System_API.Models.DTO
{
    public class ReviewInput
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }
    }
}
