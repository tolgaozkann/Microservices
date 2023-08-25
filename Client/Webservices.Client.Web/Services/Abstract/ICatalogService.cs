using Webservices.Client.Web.Models.Catalog;

namespace Webservices.Client.Web.Services.Abstract;

public interface ICatalogService
{
    Task<List<CourseViewModel>> GetAllCoursesAsycn();
    Task<List<CategoryViewModel>> GetAllCategoriesAsycn();
    Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId);
    Task<CourseViewModel> GetCourseByCourseIdAsync(string courseId);
    Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
    Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput);
    Task<bool> DeleteCourseAsync(string courseId);
}