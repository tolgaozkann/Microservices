namespace Webservices.Client.Web.Services.Abstract;

public interface IClientCredentialTokenService
{
    Task<String> GetTokenAsync();
}