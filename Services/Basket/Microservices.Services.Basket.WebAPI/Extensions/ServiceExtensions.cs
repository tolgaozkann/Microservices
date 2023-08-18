using Microservices.Services.Basket.WebAPI.Config;
using Microservices.Services.Basket.WebAPI.Services.Abstract;
using Microservices.Services.Basket.WebAPI.Services.Concrete;
using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Microservices.Services.Basket.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRedis(this IServiceCollection services,
        IConfiguration configuration)
    {
        //get redis settings from appsettings.json
        services.Configure<RedisSettings>(configuration.GetSection("RedisSettings"));

        //configure redis service
        services.AddSingleton<RedisService>(sp =>
        {
            var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

            var redisService = new RedisService(redisSettings.Host, redisSettings.Port);

            redisService.Connect();

            return redisService;
        });
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        services.AddScoped<IBasketService, BasketService>();
    }

    public static void ConfigureAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Authority = configuration["IdentityServerUrl"];
                opt.Audience = "resource_basket";
                opt.RequireHttpsMetadata = false;

            });
    }
}