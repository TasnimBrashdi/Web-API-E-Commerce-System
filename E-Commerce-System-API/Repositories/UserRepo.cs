using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(a => a.UId == id);
        }
        public User GetUser(string email, string password)
        {
            return _context.Users.Where(u => u.Email == email & u.Password == password).FirstOrDefault();
        }
        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public void UpdateUser(User user, int id)
        {
            var currenruser = GetById(id);
            if (currenruser != null)
            {
                currenruser.Name = user.Name;
                currenruser.Email = user.Email;
                currenruser.Password = user.Password;
                currenruser.Phone = user.Phone;


                _context.Users.Update(currenruser);
                _context.SaveChanges();
            }
        }
    }
}
