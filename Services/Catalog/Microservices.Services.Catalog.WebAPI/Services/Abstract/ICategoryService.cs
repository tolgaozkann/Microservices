using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Catalog.WebAPI.Services.Abstract;

public interface ICategoryService
{
    Task<ResponseDto<List<CategoryDto>>> GetAllAsync();
    Task<ResponseDto<CategoryDto>> CreateAsync(CategoryCreateDto categoryCreateDto);
    Task<ResponseDto<CategoryDto>> GetByIdAsync(string id);
    Task<ResponseDto<NoContent>> UpdateAsync(CategoryDto category);
    Task<ResponseDto<NoContent>> DeleteAsync(string id);
}