using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Services
{
    public interface IUserService
    {
        void Delete(int id);
        User GetById(int id);
        User Login(string email, string password);
        void Register(User user);
        void UpdateUser( int id, User user);
    }
}