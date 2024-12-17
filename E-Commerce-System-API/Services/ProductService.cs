using E_Commerce_System_API.Models;
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
        public ProductOutputDTO UpdateProduct(int id, ProductInputDTO inputDto)
        {
            var existingProduct = _productrepo.GetProductById(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }

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
            {
                throw new Exception("Product not found.");
            }

            return product;
        }

        public List<Product> GetProductsByName(string name)
        {
            return _productrepo.GetProductsByName(name);
        }


    }
}
