using Webservices.Client.Web.Models;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UserViewModel> GetUser()
    {
        return await _httpClient.GetFromJsonAsync<UserViewModel>("/api/User/GetUser");
    }
}