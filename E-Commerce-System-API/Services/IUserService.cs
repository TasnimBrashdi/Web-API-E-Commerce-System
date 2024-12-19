using E_Commerce_System_API.Models.DTO;
using System.Security.Claims;

namespace E_Commerce_System_API.Services
{
    public interface IUserService
    {
        void Delete(int id);
        UserOutput GetById(int id);
        UserOutput Login(string email, string password);
        void Register(UserInput user);
        void UpdateUser( int id, UserInput user);
        int GetCurrentUserId(ClaimsPrincipal user);
    }
}