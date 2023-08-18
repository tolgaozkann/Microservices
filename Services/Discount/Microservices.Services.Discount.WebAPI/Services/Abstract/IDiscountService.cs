using Microservices.Shared.Dtos;

namespace Microservices.Services.Discount.WebAPI.Services.Abstract;

public interface IDiscountService
{
    Task<ResponseDto<List<Models.Discount>>> GetAll();
    Task<ResponseDto<Models.Discount>> GetById(int id);
    Task<ResponseDto<NoContent>> Save(Models.Discount discount);
    Task<ResponseDto<NoContent>> DeleteById(int id);
    Task<ResponseDto<NoContent>> Update(Models.Discount discount);
    Task<ResponseDto<Models.Discount>> GetByCodeAndUserId(string userId, string code);
}