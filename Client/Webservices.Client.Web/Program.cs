using System.Configuration;
using Webservices.Client.Web.Config;
using Webservices.Client.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Configure Services

builder.Services.ConfigureServiceApiAndClientSettings(builder.Configuration);
builder.Services.ConfigureHttp(builder.Configuration);
builder.Services.ConfigureCokies();
builder.Services.RegisterServices();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
