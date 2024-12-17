using E_Commerce_System_API.Models;
using E_Commerce_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrederService _orderService;
        private readonly IUserService _userService; // To get user-specific data

        public OrderController(IOrederService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        // Place a new order
        [HttpPost("place-order")]
        public IActionResult PlaceOrder([FromBody] Order order)
        {
            var userId = _userService.GetCurrentUserId(User); // Fetch authenticated user ID
            order.UId = userId;

            if (_orderService.PlaceOrder(order))
            {
                return Ok(new { message = "Order placed successfully." });
            }

            return BadRequest(new { message = "Failed to place order. Insufficient stock." });
        }

        // Get all orders for an authenticated user
        [HttpGet]
        public IActionResult GetUserOrders()
        {
            var userId = _userService.GetCurrentUserId(User); // Fetch authenticated user ID
            var orders = _orderService.GetOrdersByUserId(userId);

            return Ok(orders);
        }

        // Get order details by ID for an authenticated user
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var userId = _userService.GetCurrentUserId(User); // Fetch authenticated user ID
            var order = _orderService.GetOrderByIdAndUser(id, userId);

            if (order == null)
            {
                return NotFound(new { message = "Order not found or access denied." });
            }

            return Ok(order);
        }
    }

}
