using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public interface IOrderRepo
    {
        void AddOrder(Order order);
        void DeleteOrder(int id);
        List<Order> GetAllOrder();
        Order GetOrderById(int id);
    }
}