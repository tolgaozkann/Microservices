using Microservices.Services.Order.Application.Handlers;
using Microservices.Services.Order.Infrastructure.EfCore;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        }

        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining(typeof(CreateOrderCommandHandler));
            });
        }

        public static void ConfigureAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.Authority = configuration["IdentityServerUrl"];
                    opt.Audience = "resource_order";
                    opt.RequireHttpsMetadata = false;

                });
        }
    }
}
