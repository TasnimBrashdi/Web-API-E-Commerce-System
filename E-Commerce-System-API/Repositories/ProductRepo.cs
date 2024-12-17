using E_Commerce_System_API.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_System_API.Repositories
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        //Get list of products 
        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();

        }
        // Get product by ID
        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.PId == id);
        }

        // Get product by Name
        public List<Product> GetProductsByName(string name)
        {
            return _context.Products
                .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
                .ToList();
        }

        // Update an existing product
        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.PId == updatedProduct.PId);
            if (existingProduct != null)
            {
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.Stock = updatedProduct.Stock;


                _context.SaveChanges();
            }
        }

        // Delete a product
        public void DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.PId == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
        //retrieving products with pagination and filtering
        public List<Product> GetProducts(string name, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            // Filter by name if provided
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            // Filter by price range if provided
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Apply pagination
            return query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
    }
}
