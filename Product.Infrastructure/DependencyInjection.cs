using Microsoft.AspNetCore.Identity;
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
            services.AddDbContext<ProductDbContext>(o =>
            {
                o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Product.API"));
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            });

            services.AddScoped<IProductDbContext>(provider =>
            provider.GetRequiredService<ProductDbContext>());
            services.AddIdentity<User, IdentityRole>(options => {

                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;

            }).AddEntityFrameworkStores<ProductDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddHttpContextAccessor();


            return services;
        }
    }
}
