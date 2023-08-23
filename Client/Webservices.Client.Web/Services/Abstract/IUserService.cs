using Webservices.Client.Web.Models;

namespace Webservices.Client.Web.Services.Abstract;

public interface IUserService
{
    Task<UserViewModel> GetUser();
}