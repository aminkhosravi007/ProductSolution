using Microsoft.EntityFrameworkCore;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class ProductDbContext : DbContext, IProductDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options)
        {
                
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
    }
}
