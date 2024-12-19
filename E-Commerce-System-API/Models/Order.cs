using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace E_Commerce_System_API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        [JsonIgnore]
        public int UId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public decimal TotalAmount { get; set; }
     
        public ICollection<OrderProducts> OrderProduct { get; set; } = new List<OrderProducts>();


    }

}

