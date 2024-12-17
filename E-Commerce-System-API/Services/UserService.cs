using E_Commerce_System_API.Models;
using E_Commerce_System_API.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace E_Commerce_System_API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userrepo;

        public UserService(IUserRepo userrepo)
        {
            _userrepo = userrepo;
        }


        public void Register(User user)
        {
            // Hash the password before storing it
            user.Password = HashPassword(user.Password);
            _userrepo.AddUser(user);
        }

        public User Login(string email, string password)
        {
            // Hash the input password and compare it with the stored hash
            string hashedPassword = HashPassword(password);
            return _userrepo.GetUser(email, hashedPassword);

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

        public User GetById(int id)
        {
            var user = _userrepo.GetById(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            return user;
        }
        public void UpdateUser(int id, User user)
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
