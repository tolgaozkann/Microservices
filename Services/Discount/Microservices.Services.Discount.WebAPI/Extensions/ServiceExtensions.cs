using Microservices.Services.Discount.WebAPI.Services.Abstract;
using Microservices.Services.Discount.WebAPI.Services.Concrete;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microservices.Services.Discount.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServerUrl"];
                opt.Audience = "resource_discount";
                opt.RequireHttpsMetadata = false;

            });
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
    }
}