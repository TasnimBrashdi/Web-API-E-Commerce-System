using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models.DTO
{
    public class ReviewInput
    {   //ignored during input,use for output only
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Available in output but ignored for input
        public int? Id { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;

        [Required]
        public int ProductId { get; set; }
    }
}
