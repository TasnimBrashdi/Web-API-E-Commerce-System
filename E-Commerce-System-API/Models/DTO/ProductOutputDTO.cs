namespace E_Commerce_System_API.Models.DTO
{
    public class ProductOutputDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal Overall_Rating { get; set; }
    }
}
