using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<ProductModel> AddProduct(ProductModel model)
        {
            await _repository.AddProduct(model);
            return model;
        }

        public async Task<ProductModel> DeleteProduct(int id)
        {
            return await _repository.DeleteProduct(id);
        }

        public Task<List<ProductModel>> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            return await _repository.GetProductById(id);
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            await _repository.UpdateProduct(model);
            return model;
        }
    }
}
