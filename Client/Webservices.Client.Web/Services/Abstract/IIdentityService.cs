using IdentityModel.Client;
using Microservices.Shared.Dtos;
using Webservices.Client.Web.Models;

namespace Webservices.Client.Web.Services.Abstract;

public interface IIdentityService
{
    Task<ResponseDto<bool>> SignIn(SigninInput input);
    Task<TokenResponse> GetAccessTokenByRefreshToken();
    Task RevokeRefreshToken();
}