using Microservices.Services.Catalog.WebAPI.Config;
using Microservices.Services.Catalog.WebAPI.Services.Abstract;
using Microservices.Services.Catalog.WebAPI.Services.Concrete;
using Microsoft.Extensions.Options;

namespace Microservices.Services.Catalog.WebAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureDatabaseSettings(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

        services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IServiceManager, ServiceManager>();
    }
        
}