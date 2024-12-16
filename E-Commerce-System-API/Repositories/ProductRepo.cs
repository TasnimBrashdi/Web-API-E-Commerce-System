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


    }
}
