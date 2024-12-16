using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Repositories
{
    public interface IProductRepo
    {
        void AddProduct(Product product);
        void DeleteProduct(int id);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProductsByName(string name);
        void UpdateProduct(Product updatedProduct);
    }
}