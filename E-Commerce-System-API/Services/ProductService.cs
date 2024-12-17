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

        public void AddProduct(Product product)
        {
            _productrepo.AddProduct(product);
        }
        public void UpdateProduct(int id, Product updatedProduct)
        {
            // Retrieve the existing product by ID
            var existingProduct = _productrepo.GetProductById(id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            // Update fields
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock;


            _productrepo.UpdateProduct(existingProduct);
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
