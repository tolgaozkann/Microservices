using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Services.Catalog.WebAPI.Services.Abstract;
using Microservices.Shared.ControllerBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Catalog.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : CustomControllerBase
    {
        private readonly IServiceManager _serviceManager;


        public CategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _serviceManager.CategoryService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _serviceManager.CategoryService.GetByIdAsync(id);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto categoryCreateDto)
        {
            var response = await _serviceManager.CategoryService.CreateAsync(categoryCreateDto);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto category)
        {
            var response = await _serviceManager.CategoryService.UpdateAsync(category);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _serviceManager.CategoryService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}
