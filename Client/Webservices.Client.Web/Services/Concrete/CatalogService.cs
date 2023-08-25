using Microservices.Shared.Dtos;
using Webservices.Client.Web.Models.Catalog;
using Webservices.Client.Web.Services.Abstract;

namespace Webservices.Client.Web.Services.Concrete;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _httpClient;

    public CatalogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CourseViewModel>> GetAllCoursesAsycn()
    {
        var response = await _httpClient.GetAsync("Course");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<List<CourseViewModel>>>();

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

        return successRespone.Data;
    }

    public async Task<CourseViewModel> GetCourseByCourseIdAsync(string courseId)
    {
        var response = await _httpClient.GetAsync($"Course/{courseId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var successRespone = await response.Content.ReadFromJsonAsync<ResponseDto<CourseViewModel>>();

        return successRespone.Data;
    }


    public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
    {
        var response = await _httpClient.PostAsJsonAsync("Course", courseCreateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
    {
        var response = await _httpClient.PutAsJsonAsync("Course", courseUpdateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(string courseId)
    {
        var response = await _httpClient.DeleteAsync($"Course/{courseId}");

        return response.IsSuccessStatusCode;
    }
}