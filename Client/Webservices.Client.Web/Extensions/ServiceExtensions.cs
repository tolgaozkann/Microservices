using Microservices.Shared.Services.Abstract;
using Microservices.Shared.Services.Concrete;
using Microsoft.AspNetCore.Authentication.Cookies;
using Webservices.Client.Web.Config;
using Webservices.Client.Web.Handlers;
using Webservices.Client.Web.Services.Abstract;
using Webservices.Client.Web.Services.Concrete;

namespace Webservices.Client.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServiceApiAndClientSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));
        services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
    }

    public static void ConfigureCokies(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            opt =>
            {
                opt.LoginPath = "/Auth/SignIn";
                opt.ExpireTimeSpan = TimeSpan.FromDays(60);
                opt.SlidingExpiration = true;
                opt.Cookie.Name = "micorservicescookie";
            });
    }

    public static void ConfigureHttp(this IServiceCollection services,IConfiguration configuration)
    {
        var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

        services.AddHttpContextAccessor();

        //configure IdentityServer4 microservice
        services.AddHttpClient<IIdentityService,IdentityService>();

        //user service
        services.AddHttpClient<IUserService, UserService>(opt =>
        {
            opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
        }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

        //credential service
        services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

        //configure catalog microservice
        services.AddHttpClient<ICatalogService, CatalogService>(opt =>
        {
            opt.BaseAddress = new Uri($"{serviceApiSettings.IdentityBaseUri}/{serviceApiSettings.Catalog.Path}");
        }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ResourceOwnerPasswordTokenHandler>();
        services.AddScoped<ClientCredentialTokenHandler>();
        services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        services.AddAccessTokenManagement();
    }
}