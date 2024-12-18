using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_System_API.Services
{
    public class OrederService : IOrederService
    {
        private readonly IOrderRepo _OrderRepository;
        private readonly IProductRepo _ProductService;


        public OrederService(IOrderRepo OrderRepository, IProductRepo productService)
        {
            _OrderRepository = OrderRepository;
            _ProductService = productService;
        }
        public bool PlaceOrder(int userId, List<(int ProductId, int Quantity)> products)
        {// Create a new order
            var order = new Order
            {
                UId = userId,
                OrderDate = DateTime.UtcNow,
                OrderProduct = new List<OrderProducts>()
            };
            // Add products to the order,Validate stock availability
            foreach (var productInput in products)
            {
                var product = _ProductService.GetProductById(productInput.ProductId);

                if (product == null || product.Stock < productInput.Quantity)
                {
                    return false; // Insufficient stock or invalid product
                }
                // Add product to order
                order.OrderProduct.Add(new OrderProducts
                {
                    ProductId = product.PId,
                    Quantity = productInput.Quantity
                });

            }
                // Calculate total amount
                order.TotalAmount = order.OrderProduct.Sum(op =>
            {
                var product = _ProductService.GetProductById(op.ProductId);
                return product.Price * op.Quantity;
            });

            // Update stock
            foreach (var orderProduct in order.OrderProduct)
            {
                var product = _ProductService.GetProductById(orderProduct.ProductId);
                product.Stock -= orderProduct.Quantity;



                _ProductService.UpdateProduct( product);

            }


            // Save the order
            _OrderRepository.AddOrder(order);
            return true; // Order successfully placed
        }
        // Get all orders for a specific user
        public List<Order> GetOrdersByUserId(int userId)
        {
            return _OrderRepository.GetAllOrder().Where(o => o.UId == userId).ToList();
        }

        // Get order details by ID and user ID
        public Order GetOrderByIdAndUser(int id, int userId)
        {
            var order = _OrderRepository.GetOrderById(id);
            if (order != null && order.UId == userId)
            {
                return order;
            }

            return null; // Either order doesn't exist or access denied
        }

        public List<Order> GetAllOrders()
        {
            return _OrderRepository.GetAllOrder();
        }

        public Order GetOrderById(int id)
        {
            return _OrderRepository.GetOrderById(id);
        }

        public void DeleteOrder(int id)
        {
            _OrderRepository.DeleteOrder(id);
        }
        // Method to check if the user has purchased a product
        public bool HasUserPurchasedProduct(int userId, int productId)
        {
            return _OrderRepository.HasUserPurchasedProduct(userId, productId);
        }
    }
}
