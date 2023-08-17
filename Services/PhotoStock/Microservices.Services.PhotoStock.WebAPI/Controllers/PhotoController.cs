using Microservices.Services.PhotoStock.WebAPI.Dtos;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.PhotoStock.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoController : CustomControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo is null && photo.Length <= 0)
                return CreateActionResultInstance(ResponseDto<PhotoDto>.Fail("Invalid file", 400));

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await photo.CopyToAsync(stream, cancellationToken);
            }

            var returnPath = $"photos/{photo.FileName}";

            var photoDto = new PhotoDto()
            {
                Url = returnPath,
            };

            return CreateActionResultInstance(ResponseDto<PhotoDto>.Success(photoDto, 200));
        }

        [HttpGet]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/", photoUrl);
            if (!System.IO.File.Exists(path))
                return CreateActionResultInstance(ResponseDto<NoContent>.Fail("Photo Not Found.", 404));

            System.IO.File.Delete(path);
            return CreateActionResultInstance(ResponseDto<NoContent>.Success(204));
        }
    }
}
