using Ocelot.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json")
    .AddEnvironmentVariables();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationScheme", opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "resource_gateway";
    opt.RequireHttpsMetadata = false;
});

builder.Services.AddOcelot();



var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseDeveloperExceptionPage();

app.Run();
