using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce_System_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IConfiguration configuration, ILogger<UserController> logger)
        {
            _userService = userService;
            _configuration = configuration;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpPost]
      
        public IActionResult AddUser([FromQuery] UserInput user)
        {
            try
            {
                _userService.Register(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(user.Name);
        }
        [AllowAnonymous]
        [HttpGet("LogInUser")]
        public IActionResult loginUser(string email, string password)
        {
            try
            {
                var user = _userService.Login(email, password);

                if (user != null)
                {
                    string token = GenerateJwtToken(user.UId.ToString(), user.Name, user.Role);
                    var response = new
                    {
                        Token = token,
                        ExpiresIn = 3600 // Token expiration in seconds
                    };
                    return Ok(token);

                }
                else
                {
                    return BadRequest("Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user.");
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
        [NonAction]
        public string GenerateJwtToken(string userId, string username,string role)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.UniqueName, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
           new Claim(ClaimTypes.Role, role), // Adding the role claim
           new Claim(ClaimTypes.NameIdentifier, userId)

    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [Authorize(Roles = "admin")]//Allow only admin  
        [HttpGet("GetUserById")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                // Get user ID from token
                var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdFromToken == null)
                {
                    return Unauthorized("User ID not found in the token.");
                }
                // Fetch user details by the provided ID
                var user = _userService.GetById(id);

             
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {// Log the error and return a meaningful message
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUserInfo(int id, string name, string email, string phone, string Password)
        {
            try
            {
                _userService.UpdateUser(id, new UserInput
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    Password = Password
       
                });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]//Allow only admin to Delete  
        [HttpDelete("{id}")]
        public IActionResult DeleteAdmin(int id)
        {
            try
            {
                _userService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
