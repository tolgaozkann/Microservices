using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Services.Catalog.WebAPI.Services.Abstract;
using Microservices.Shared.ControllerBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Catalog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CourseController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var response = await _serviceManager.CourseService.GetAllAsync();

            return CreateActionResultInstance(response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _serviceManager.CourseService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpGet]
        [Route("/api/[controller]/GetByUserId/{userId}")]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId)
        {
            var response = await _serviceManager.CourseService.GetAllByUserIdAsync(userId);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CourseCreateDto courseDto)
        {
            var response = await _serviceManager.CourseService.CreateAsync(courseDto);

            return CreateActionResultInstance(response);

        }

        [HttpPut]
        public async Task<IActionResult> CreateCourse(CourseUpdateDto courseDto)
        {
            var response = await _serviceManager.CourseService.UpdateAsync(courseDto);

            return CreateActionResultInstance(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(string id)
        {
            var response = await _serviceManager.CourseService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}
