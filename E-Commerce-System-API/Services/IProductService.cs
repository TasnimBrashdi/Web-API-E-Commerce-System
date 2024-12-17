using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Services
{
    public interface IProductService
    {
        void AddProduct(Product product);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProductsByName(string name);
        void RemoveProduct(int ID);
        void UpdateProduct(int id, Product updatedProduct);
    }
}