using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models
{
    [PrimaryKey(nameof(OrderId), nameof(ProductId))]
    public class OrderProducts
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative integer.")]
        public int Quantity { get; set; }   

    }
}
