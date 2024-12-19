using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Repositories;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static E_Commerce_System_API.Models.DTO.UserInput;

namespace E_Commerce_System_API.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepo _userrepo;

        public UserService(IUserRepo userrepo)
        {
            _userrepo = userrepo;
        }


        public void Register(UserInput userInput)
        {
          
            var user = new User
            {
                Name = userInput.Name,
                Email = userInput.Email,
                Password = HashPassword(userInput.Password),  // Hash the password before storing it
                Phone = userInput.Phone,
                Role = userInput.Role
            };
            _userrepo.AddUser(user);

          
        }
        private UserOutput MapToOutputModel(User user) { 
        
            return new UserOutput
            {
                UId = user.UId,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
        public UserOutput Login(string email, string password)
        {
            // Hash the input password and compare it with the stored hash
            string hashedPassword = HashPassword(password);
            var user = _userrepo.GetUser(email, hashedPassword);

            if (user == null)
            {
                throw new Exception("Invalid email or password.");
            }

            return MapToOutputModel(user);
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create()) //uses SHA-256 to hash the password securely.
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2")); // Convert byte to hexadecimal
                }
                return builder.ToString();
            }
        }

        public UserOutput GetById(int id)
        {
            var user = _userrepo.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return MapToOutputModel(user);
        }
        public void UpdateUser(int id, UserInput user)
        {
            var existingUser = _userrepo.GetById(id);
            if (existingUser == null)
            {
                throw new Exception("User not found.");
            }

            // Update fields
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;

            // If password is provided, hash it before updating
            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = HashPassword(user.Password);
            }

            _userrepo.UpdateUser(existingUser, id);
        }

        public void Delete(int id)
        {
            var user = _userrepo.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            _userrepo.Delete(id);


        }
        public int GetCurrentUserId(ClaimsPrincipal user)
        {
            if (user == null || !user.Identity.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            // Debugging: Inspect available claims
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            // Assuming the user ID is stored as a claim named "id"
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id" || c.Type == "user_id" || c.Type == "sub");
            if (userIdClaim == null)
            {
                var availableClaims = string.Join(", ", user.Claims.Select(c => $"{c.Type} ({c.Value})"));
                throw new KeyNotFoundException($"User ID claim not found. Available claims: {availableClaims}");
            }

            if (int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            throw new FormatException("Invalid User ID format.");
        }
    }
}
