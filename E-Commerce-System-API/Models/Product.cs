using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models
{
    public class Product
    {
        [Key]
        [JsonIgnore]
        public int PId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; } 
        [Required]
        [Range(0, 1000, ErrorMessage = "Price should be from 0 to 1000")]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }

        public decimal Overall_Rating { get; set; } 

    }
}
