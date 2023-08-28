using Webservices.Client.Web.Models.PhotoStock;

namespace Webservices.Client.Web.Services.Abstract
{
    public interface IPhotoStockService
    {
        Task<PhotoStockViewModel> Upload(IFormFile photo);
        Task<bool> Remove(string photoUrl);
    }
}
