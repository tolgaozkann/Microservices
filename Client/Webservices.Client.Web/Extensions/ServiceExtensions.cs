using Webservices.Client.Web.Config;

namespace Webservices.Client.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServiceApiAndClientSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));
        services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
    }
}