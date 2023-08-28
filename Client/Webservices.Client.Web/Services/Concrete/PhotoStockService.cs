using Microservices.Shared.Dtos;
using Webservices.Client.Web.Models.PhotoStock;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<bool> Remove(string photoUrl)
        {
            var response = await _httpClient.DeleteAsync($"Photo?=photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoStockViewModel> Upload(IFormFile photo)
        {
            if (photo is null || photo.Length <= 0)
                return null;
            
            
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";

            using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);

            var multipartContent = new MultipartFormDataContent();

            multipartContent.Add(new ByteArrayContent(ms.ToArray()),"photo",randomFileName);

            var response = await _httpClient.PostAsync("Photo", multipartContent);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseDto = await response.Content.ReadFromJsonAsync<ResponseDto<PhotoStockViewModel>>();

            return responseDto.Data;
        }
    }
}
