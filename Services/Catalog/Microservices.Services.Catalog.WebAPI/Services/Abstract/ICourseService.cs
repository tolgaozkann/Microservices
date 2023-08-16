using Microservices.Services.Catalog.WebAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Catalog.WebAPI.Services.Abstract;

public interface ICourseService
{
    Task<ResponseDto<List<CourseDto>>> GetAllAsync();
    Task<ResponseDto<CourseDto>> CreateAsync(CourseCreateDto courseCreateDto);
    Task<ResponseDto<CourseDto>> GetByIdAsync(string id);
    Task<ResponseDto<List<CourseDto>>> GetAllByUserIdAsync(string userId);
    Task<ResponseDto<NoContent>> UpdateAsync(CourseUpdateDto courseUpdateDto);
    Task<ResponseDto<NoContent>> DeleteAsync(string id);
}