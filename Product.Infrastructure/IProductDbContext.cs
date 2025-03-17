using Microsoft.EntityFrameworkCore;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public interface IProductDbContext
    {
        public DbSet<Domain.ProductModel> Products { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }    
    }
}
