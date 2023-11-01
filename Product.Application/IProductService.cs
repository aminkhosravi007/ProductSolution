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
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
        Task<ProductModel> AddProduct(ProductModel model);
        Task<ProductModel> UpdateProduct(ProductModel model);
        Task DeleteProduct(int id);
    }
}
