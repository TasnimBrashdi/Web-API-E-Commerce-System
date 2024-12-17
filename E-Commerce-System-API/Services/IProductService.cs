using E_Commerce_System_API.Models;

namespace E_Commerce_System_API.Services
{
    public interface IProductService
    {
        ProductOutputDTO AddProduct(ProductInputDTO inputDto);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProductsByName(string name);
        void RemoveProduct(int ID);
        ProductOutputDTO UpdateProduct(int id, ProductInputDTO inputDto);
    }
}