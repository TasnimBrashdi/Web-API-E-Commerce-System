using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Services
{
    public interface IOrederService
    {
        void DeleteOrder(int id);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        bool PlaceOrder(Order order);
    }
}