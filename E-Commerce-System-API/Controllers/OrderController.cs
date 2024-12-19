using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_System_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrederService _orderService;
        private readonly IUserService _userService; // To get user-specific data
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrederService orderService, IUserService userService, ILogger<OrderController> logge)
        {
            _orderService = orderService;
            _userService = userService;
            _logger = logge;
        }


        // Place a new order
        [HttpPost("place-order")]
        public IActionResult PlaceOrder([FromBody] PlaceOrderDto order)
        {
            try {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Fetch authenticated user ID
                                                                               // Ensure the userId is a valid integer
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new { message = "Invalid user ID in token." });
                }
                // Call the service to place the order                                                  // Call the service to place the order                                                        // Check for invalid data (ModelState, order object, or products list)
                if (!ModelState.IsValid || order == null || order.Products == null || !order.Products.Any())
                {
                    return BadRequest(new { message = "Invalid order data." });
                }

                
                // Call the service to place the order
                var isOrderPlaced = _orderService.PlaceOrder(userId, order.Products.Select(p =>
                    (p.ProductId, p.Quantity)).ToList());

                if (isOrderPlaced)
                {
         
                    return Ok(new { message = "Order placed successfully." });
                }
                // If order placement fails
                return BadRequest(new { message = "Failed to place order. Insufficient stock or invalid products." });
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

            // Get all orders for an authenticated user
        [HttpGet("GetUserOrder")]
        public IActionResult GetUserOrders()
        {
            // Fetch authenticated user ID directly from the claims
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            // Attempt to parse the userId to an integer
            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest(new { message = "Invalid User ID format." });
            }

            // Fetch orders by user ID
            var orders = _orderService.GetOrdersByUserId(userId);

            return Ok(orders);
        } 

        // Get order details by ID for an authenticated user
        [HttpGet("Get{id}")]
        public IActionResult GetOrderById(int id)
        {
            try
            { // Fetch authenticated user ID directly from the claims
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                //var userId = _userService.GetCurrentUserId(User); 
                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }

                // Attempt to parse the userId to an integer
                if (!int.TryParse(userIdString, out int userId))
                {
                    return BadRequest(new { message = "Invalid User ID format." });
                }

                var order = _orderService.GetOrderByIdAndUser(id, userId);

          
                return Ok(new { success = true, order });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders for user {UserId}.", _userService.GetCurrentUserId(User));
                return StatusCode(500, new { success = false, message = "An unexpected error occurred." });
            }
        }
    }

}
