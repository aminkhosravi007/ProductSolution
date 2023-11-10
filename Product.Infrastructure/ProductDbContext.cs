using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public class ProductDbContext : IdentityDbContext<User>, IProductDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options)
        {
                
        }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Manufacture>().HasData(
                new Manufacture
                {
                    Id = 1,
                    ManufactureEmail = "khosh@gmail.com",
                    ManufacturePhone = "02134121995"
                },
                new Manufacture
                {
                    Id=2,
                    ManufactureEmail = "khosh@yahoo.com",
                    ManufacturePhone = "02154144444"
                },
                new Manufacture
                {
                    Id = 3,
                    ManufactureEmail = "nadin@yahoo.com",
                    ManufacturePhone = "02154144344"
                }
                );
        }
    }
}
