using Microservices.Shared.Dtos;
using Webservices.Client.Web.Helpers;
using Webservices.Client.Web.Models.Catalog;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;
    private readonly IPhotoStockService _photoStockService;
    private readonly PhotoHelper _photoHelper;

    public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
    {
        _httpClient = httpClient;
        _photoStockService = photoStockService;
        _photoHelper = photoHelper;
    }

    public async Task<List<CourseViewModel>> GetAllCoursesAsycn()
    {
        var response = await _httpClient.GetAsync("Course");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

        successRespone.Data.ForEach(x =>
        {
            x.PhotoStockPicture = _photoHelper.GetPhotoStockUrl(x.Picture);
        });


        return successRespone.Data;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoriesAsycn()
    {
        var response = await _httpClient.GetAsync("Category");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<List<CategoryViewModel>>>();



        return successRespone.Data;
    }

    public async Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"Course/GetByUserId/{userId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

        successRespone.Data.ForEach(x =>
        {
            x.PhotoStockPicture = _photoHelper.GetPhotoStockUrl(x.Picture);
        });

        return successRespone.Data;
    }

    public async Task<CourseViewModel> GetCourseByCourseIdAsync(string courseId)
    {
        var response = await _httpClient.GetAsync($"Course/{courseId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<CourseViewModel>>();
        successRespone.Data.PhotoStockPicture = _photoHelper.GetPhotoStockUrl(successRespone.Data.Picture);

        return successRespone.Data;
    }


    public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
    {
        var resultPhotoStockService = await _photoStockService.Upload(courseCreateInput.PhotoFormFile);

        if (resultPhotoStockService is not null){}
            courseCreateInput.Picture = resultPhotoStockService.Url;

        var response = await _httpClient.PostAsJsonAsync("Course", courseCreateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
    {
        var resultPhotoStockService = await _photoStockService.Upload(courseUpdateInput.PhotoFormFile);

        if (resultPhotoStockService is not null)
        {
            await _photoStockService.Remove(courseUpdateInput.Picture);
            courseUpdateInput.Picture = resultPhotoStockService.Url;
        }
            

        var response = await _httpClient.PutAsJsonAsync("Course", courseUpdateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        
        var response = await _httpClient.DeleteAsync($"Course/{courseId}");

        return response.IsSuccessStatusCode;
    }
}