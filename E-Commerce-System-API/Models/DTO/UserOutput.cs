namespace E_Commerce_System_API.Models.DTO
{
    public class UserOutput
    {
        public int UId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
