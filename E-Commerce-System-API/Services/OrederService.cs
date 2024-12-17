using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Repositories;

namespace E_Commerce_System_API.Services
{
    public class OrederService : IOrederService
    {
        private readonly IOrderRepo _OrderRepository;
        private readonly IProductService _ProductService;

        public OrederService(IOrderRepo OrderRepository, IProductService productService)
        {
            _OrderRepository = OrderRepository;
            _ProductService = productService;
        }
        public bool PlaceOrder(Order order)
        {
            // Validate stock availability
            foreach (var orderProduct in order.OrderProduct)
            {
                var product = _ProductService.GetProductById(orderProduct.ProductId);
                if (product == null || product.Stock < orderProduct.Quantity)
                {
                    return false; // Insufficient stock
                }
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
                var updatedProductDto = new ProductInputDTO
                {
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock - orderProduct.Quantity
                };

                _ProductService.UpdateProduct(product.PId, updatedProductDto);
            }

            // Save the order
            _OrderRepository.AddOrder(order);
            return true; // Order successfully placed
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
    }
}
