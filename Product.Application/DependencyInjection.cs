using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Product.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection serviceDescriptors(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(typeof(Mapping));
            services.AddScoped<IProductService, ProductService>();


            return services;
        }
    }
}
