using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;

namespace E_Commerce_System_API.Services
{
    public interface IProductService
    {
        ProductOutputDTO AddProduct(ProductInputDTO inputDto);
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        List<Product> GetProductsByName(string name);
        void RemoveProduct(int ID);
        ProductOutputDTO UpdateProducts(int id, ProductInputDTO inputDto);
        List<ProductOutputDTO> GetProducts(string name, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
    }
}