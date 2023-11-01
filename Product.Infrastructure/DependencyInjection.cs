using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection ImplementDataInject(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(o => o.UseSqlServer(configuration.GetConnectionString
                ("DefaultConnection"), b => b.MigrationsAssembly("Product.API")), ServiceLifetime.Transient);

            services.AddScoped<IProductDbContext>(provider =>
            provider.GetRequiredService<ProductDbContext>());
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
