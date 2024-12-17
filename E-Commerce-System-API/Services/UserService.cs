using E_Commerce_System_API.Models;
using E_Commerce_System_API.Repositories;
using System.Security.Cryptography;
using System.Text;
using static E_Commerce_System_API.Models.UserInput;

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
    }
}
