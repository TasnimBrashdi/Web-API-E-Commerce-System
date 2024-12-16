using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public interface IUserRepo
    {
        void AddUser(User user);
        void Delete(int id);
        User GetById(int id);
        User GetUser(string email, string password);
        void UpdateUser(User user, int id);
    }
}