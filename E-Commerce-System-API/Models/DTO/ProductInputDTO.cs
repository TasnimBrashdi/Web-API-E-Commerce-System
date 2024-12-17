using System.ComponentModel.DataAnnotations;

namespace E_Commerce_System_API.Models.DTO
{
    public class ProductInputDTO
    {
        [Required]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        [Range(0, 1000, ErrorMessage = "Price should be from 0 to 1000")]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a non-negative integer.")]
        public int Stock { get; set; }
    }
}
