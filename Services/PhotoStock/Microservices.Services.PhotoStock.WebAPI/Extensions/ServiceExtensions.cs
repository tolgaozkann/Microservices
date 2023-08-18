using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microservices.Services.PhotoStock.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServerUrl"];
                opt.Audience = "resource_photo_stock";
                opt.RequireHttpsMetadata = false;
            });
    }
}