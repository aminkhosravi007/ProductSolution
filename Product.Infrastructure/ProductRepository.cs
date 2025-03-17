using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Product.Application;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public ProductRepository(ProductDbContext dbContext, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _contextAccessor = contextAccessor;
        }
        public async Task<Domain.ProductModel> AddProduct(Domain.ProductModel model)
        {
            model.IssuedAdminToken = _contextAccessor.HttpContext.Session.GetString("email");
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

        public async Task<List<Domain.ProductModel>> GetAllProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<Domain.ProductModel> GetProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product;

        }

        public async Task<Domain.ProductModel> UpdateProduct(Domain.ProductModel model)
        {
            _dbContext.Products.Update(model);
            _dbContext.Entry(model).Property(x => x.IssuedAdminToken).IsModified = false;
            await _dbContext.SaveChangesAsync();
            return model;
        }
        public async Task<List<Domain.ProductModel>> GetProductsIssuedByAdmin(string adminEmail)
        {
            var products = await _dbContext.Products.Where(p=> p.IssuedAdminToken.Contains(adminEmail)).ToListAsync();
            return products;
        }
    }
}
