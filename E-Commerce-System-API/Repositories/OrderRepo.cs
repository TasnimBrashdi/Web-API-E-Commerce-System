using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        //Get list of order 
        public List<Order> GetAllOrder()
        {
            return _context.Orders.ToList();

        }
        // Get order by ID
        public Order GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(p => p.Id == id);
        }
        //delete order
        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(p => p.Id == id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

    }
}
