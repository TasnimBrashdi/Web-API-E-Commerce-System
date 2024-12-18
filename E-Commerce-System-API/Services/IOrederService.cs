using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Services
{
    public interface IOrederService
    {
        void DeleteOrder(int id);
        List<Order> GetAllOrders();
        Order GetOrderById(int id);
        Order GetOrderByIdAndUser(int id, int userId);
        List<Order> GetOrdersByUserId(int userId);
        bool PlaceOrder(Order order);
        bool HasUserPurchasedProduct(int userId, int productId);
    }
}