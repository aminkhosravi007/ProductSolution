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
        public async Task<Domain.ProductModel> AddProduct(Domain.ProductModel model)
        {
            await _repository.AddProduct(model);
            return model;
        }

        public async Task DeleteProduct(int id)
        {
            await _repository.DeleteProduct(id);
        }

        public Task<List<Domain.ProductModel>> GetAllProducts()
        {
            return _repository.GetAllProducts();
        }

        public async Task<Domain.ProductModel> GetProductById(int id)
        {
            return await _repository.GetProductById(id);
        }

        public async Task<Domain.ProductModel> UpdateProduct(Domain.ProductModel model)
        {
            await _repository.UpdateProduct(model);
            return model;
        }
        public async Task<List<Domain.ProductModel>> GetProductsIssuedByAdmin(string adminEmail)
        {
           return await _repository.GetProductsIssuedByAdmin(adminEmail);

        }
    }
}
