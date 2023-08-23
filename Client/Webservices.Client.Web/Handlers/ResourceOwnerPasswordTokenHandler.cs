using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Webservices.Client.Web.Exceptions;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Handlers;

public class ResourceOwnerPasswordTokenHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IIdentityService _identityService;
    private readonly ILogger<ResourceOwnerPasswordTokenHandler> _logger;

    public ResourceOwnerPasswordTokenHandler(IHttpContextAccessor httpContextAccessor, 
        IIdentityService identityService, ILogger<ResourceOwnerPasswordTokenHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _identityService = identityService;
        _logger = logger;
    }

    protected  override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //get access token from HttpContext
        var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        //add jwt to headers
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //
        var response = await base.SendAsync(request,cancellationToken);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            var tokenResponse = await _identityService.GetAccessTokenByRefreshToken();

            if (tokenResponse is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
            }
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            //Error
            throw new UnauthorizeException();
        }

        return response;

    }
}