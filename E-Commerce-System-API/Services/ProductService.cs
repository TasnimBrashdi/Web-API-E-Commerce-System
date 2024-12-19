using E_Commerce_System_API.Models;
using E_Commerce_System_API.Models.DTO;
using E_Commerce_System_API.Repositories;
using Microsoft.AspNetCore.Components.Forms;

namespace E_Commerce_System_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productrepo;

        public ProductService(IProductRepo productrepo)
        {
            _productrepo = productrepo;
        }

        public ProductOutputDTO AddProduct(ProductInputDTO inputDto)
        {
            var product = new Product
            {
                Name = inputDto.Name,
                Price = inputDto.Price,
                Description = inputDto.Description,
                Stock = inputDto.Stock,



            };

            _productrepo.AddProduct(product);


            return new ProductOutputDTO
            {
                Name = product.Name,


            };
        }
        public ProductOutputDTO UpdateProducts(int id, ProductInputDTO inputDto)
        {
            var existingProduct = _productrepo.GetProductById(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }
            // Update product fields
            existingProduct.Name = inputDto.Name;
            existingProduct.Description = inputDto.Description;
            existingProduct.Price = inputDto.Price;
            existingProduct.Stock = inputDto.Stock;

            _productrepo.UpdateProduct(existingProduct);

            return new ProductOutputDTO
            {

                Name = existingProduct.Name,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                Stock = existingProduct.Stock
            };
        }

        public void RemoveProduct(int ID)
        {
            var product = _productrepo.GetProductById(ID);
            if (product == null)
            {
                throw new Exception("product not found.");
            }

            _productrepo.DeleteProduct(ID);
        }

        public List<Product> GetAllProducts()
        {
            return _productrepo.GetAllProducts();
        }

        public Product GetProductById(int id)
        {
            var product = _productrepo.GetProductById(id);
            if (product == null)
            {//crash
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            return product;
        }

        public List<Product> GetProductsByName(string name)
        {
            return _productrepo.GetProductsByName(name);
        }

        public List<ProductOutputDTO> GetProducts(string name, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var products = _productrepo.GetProducts(name, minPrice, maxPrice, pageNumber, pageSize);

            // Map products to DTOs
            return products.Select(p => new ProductOutputDTO
            {
              
                Name = p.Name,
                Description = p.Description, 
                Price = p.Price,
                Stock = p.Stock
            }).ToList();
        }
       


    }
}
