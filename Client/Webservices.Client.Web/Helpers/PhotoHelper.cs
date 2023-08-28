using Microsoft.Extensions.Options;
using Webservices.Client.Web.Config;

namespace Webservices.Client.Web.Helpers;

public class PhotoHelper
{
    private readonly ServiceApiSettings _serviceApiSettings;

    public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
    {
        _serviceApiSettings = serviceApiSettings.Value;
    }

    public string GetPhotoStockUrl(string photoUrl)
    {
        return $"{_serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
    }


}