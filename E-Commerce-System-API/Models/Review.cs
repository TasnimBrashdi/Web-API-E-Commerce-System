using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }// Navigation properties
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }// Navigation properties

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        public string? Comment { get; set; }// optional
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

      
    }
}
