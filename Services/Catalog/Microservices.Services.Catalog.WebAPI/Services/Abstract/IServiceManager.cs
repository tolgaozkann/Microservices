namespace Microservices.Services.Catalog.WebAPI.Services.Abstract;

public interface IServiceManager
{
    public ICategoryService CategoryService { get;}
    public ICourseService CourseService { get; }
}