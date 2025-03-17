using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public interface IProductService
    {
        Task<List<Domain.ProductModel>> GetAllProducts();
        Task<Domain.ProductModel> GetProductById(int id);
        Task<Domain.ProductModel> AddProduct(Domain.ProductModel model);
        Task<Domain.ProductModel> UpdateProduct(Domain.ProductModel model);
        Task DeleteProduct(int id);
        Task<List<Domain.ProductModel>> GetProductsIssuedByAdmin(string adminEmail);
    }
}
