using E_Commerce_System_API.Models;
using E_Commerce_System_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_System_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductController: ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        public ProductController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPost]
        [Authorize(Roles ="admin")]//Allow only admin to add product 
        public IActionResult AddProduct([FromQuery] ProductInputDTO inputDto)
        {
            //hecks if the data received in the HTTP request is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _productService.AddProduct(inputDto);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Name }, result);
        }
        [HttpGet("products")]
        public IActionResult GetProducts( [FromQuery] string? name,[FromQuery] decimal? minPrice,[FromQuery] decimal? maxPrice,[FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var products = _productService.GetProducts(name, minPrice, maxPrice, pageNumber, pageSize);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]//Allow only admin to update product 
        public IActionResult UpdateProduct(int id, [FromQuery] ProductInputDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Call the service layer to update the product
                var updatedProduct = _productService.UpdateProduct(id, inputDto);
                return Ok(updatedProduct); // Return the updated product
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productService.RemoveProduct(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

    }
}
