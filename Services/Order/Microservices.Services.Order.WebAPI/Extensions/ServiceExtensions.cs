using Microservices.Services.Order.Infrastructure.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("Microservices.Services.Order.Infrastructure");
                });
            });
        }
    }
}
