using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ProductModel> AddProduct(ProductModel model)
        {
            await _dbContext.Products.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product;

        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            _dbContext.Products.Update(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}
