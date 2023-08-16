using Microservices.Services.Catalog.WebAPI.Services.Abstract;

namespace Microservices.Services.Catalog.WebAPI.Services.Concrete;

public class ServiceManager : IServiceManager
{
    private readonly ICourseService _courseService;
    private readonly ICategoryService _categoryService;

    public ServiceManager(ICourseService courseService, ICategoryService categoryService)
    {
        _courseService = courseService;
        _categoryService = categoryService;
    }

    public ICategoryService CategoryService  => _categoryService;
    public ICourseService CourseService => _courseService;
}