using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel.Client;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Webservices.Client.Web.Config;
using Webservices.Client.Web.Models;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete;

public class IdentityService : IIdentityService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClientSettings _clientSettings;
    private readonly ServiceApiSettings _serviceApiSettings;

    public IdentityService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor,
        IOptions<ClientSettings> clientSettings, IOptions<ServiceApiSettings> serviceApiSettings)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
        _clientSettings = clientSettings.Value;
        _serviceApiSettings = serviceApiSettings.Value;
    }

    public async Task<ResponseDto<bool>> SignIn(SigninInput input)
    {
        //discovery endpoint
        var disco = await GetDisco();

        //client credentials
        var passwordTokenRequest = new PasswordTokenRequest
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            UserName = input.Email,
            Password = input.Password,
            Address = disco.TokenEndpoint
        };

        //take token
        var token = await _httpClient.RequestPasswordTokenAsync(passwordTokenRequest);

        if (token.IsError)
        {
            var responseContent = await token.HttpResponse.Content.ReadAsStringAsync();

            var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return ResponseDto<bool>.Fail(errorDto.Errors, 400);
        }

        //take user info
        var userInfoRequest = new UserInfoRequest
        {
            Token = token.AccessToken,
            Address = disco.UserInfoEndpoint,
        };

        var userInfoResponse = await _httpClient.GetUserInfoAsync(userInfoRequest);

        if (userInfoResponse.IsError)
            throw userInfoResponse.Exception;

        //get claims and use for cookie
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfoResponse.Claims, CookieAuthenticationDefaults.AuthenticationScheme,
            "name", "role");

        ClaimsPrincipal claimsPrinciple = new ClaimsPrincipal(claimsIdentity);

        //set tokens into cookie
        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.StoreTokens(new List<AuthenticationToken>()
        {
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = token.AccessToken
            },
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = token.RefreshToken
            },
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)
            }
        });

        authenticationProperties.IsPersistent = input.RememberMe;

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrinciple, authenticationProperties);

        return ResponseDto<bool>.Success(200);
    }

    public async Task<TokenResponse> GetAccessTokenByRefreshToken()
    {
        var disco = await GetDisco();

        var refreshToken =
            await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        RefreshTokenRequest refreshTokenRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            RefreshToken = refreshToken,
            Address = disco.TokenEndpoint
        };

        var token = await _httpClient.RequestRefreshTokenAsync(refreshTokenRequest);

        if (token.IsError)
            return null;

        var authenticationTokens = new List<AuthenticationToken>()
        {
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.AccessToken,
                Value = token.AccessToken
            },
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.RefreshToken,
                Value = token.RefreshToken
            },
            new AuthenticationToken
            {
                Name = OpenIdConnectParameterNames.ExpiresIn,
                Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("O",CultureInfo.InvariantCulture)
            }
        };

        var authenticationResult = await _httpContextAccessor.HttpContext.AuthenticateAsync();

        var properties = authenticationResult.Properties;

        properties.StoreTokens(authenticationTokens);

        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            authenticationResult.Principal, properties);

        return token;

    }

    public async Task RevokeRefreshToken()
    {
        var disco = await GetDisco();

        var refreshToken =
            await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

        TokenRevocationRequest tokenRevocationRequest = new()
        {
            ClientId = _clientSettings.WebClientForUser.ClientId,
            ClientSecret = _clientSettings.WebClientForUser.ClientSecret,
            Address = disco.RevocationEndpoint,
            Token = refreshToken,
            TokenTypeHint = OpenIdConnectParameterNames.RefreshToken
        };

        await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
    }

    public async Task<DiscoveryDocumentResponse> GetDisco()
    {
        var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
        {
            Address = _serviceApiSettings.IdentityBaseUri,
            Policy = new DiscoveryPolicy { RequireHttps = false }
        });

        if (disco.IsError)
            throw disco.Exception;
        return disco;
    }

    
}