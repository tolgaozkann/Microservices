using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Webservices.Client.Web.Config;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete;

public class ClientCredentialTokenService : IClientCredentialTokenService
{
    private readonly ServiceApiSettings _serviceApiSettings;
    private readonly HttpClient _httpClient;
    private readonly ClientSettings _clientSettings;
    private readonly IClientAccessTokenCache _clientAccessTokenCache;

    public ClientCredentialTokenService(IOptions<ServiceApiSettings> serviceApiSettings, IOptions<ClientSettings> clientSettings, IClientAccessTokenCache clientAccessTokenCache, HttpClient httpClient)
    {
        _serviceApiSettings = serviceApiSettings.Value;
        _clientSettings = clientSettings.Value;
        _clientAccessTokenCache = clientAccessTokenCache;
        _httpClient = httpClient;
    }

    public async Task<string> GetTokenAsync()
    {
        var currentToken = await _clientAccessTokenCache.GetAsync("WebClient",
            new ClientAccessTokenParameters());

        if (currentToken is not null)
            return currentToken.AccessToken;

        var disco = await GetDisco();

        var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
        {
            ClientId = _clientSettings.WebClient.ClientId,
            ClientSecret = _clientSettings.WebClient.ClientId,
            Address = disco.TokenEndpoint,
        };

        var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

        if(newToken.IsError)
            throw newToken.Exception;

        await _clientAccessTokenCache.SetAsync("WebClient", newToken.AccessToken, newToken.ExpiresIn,
            new ClientAccessTokenParameters());

        return newToken.AccessToken;
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