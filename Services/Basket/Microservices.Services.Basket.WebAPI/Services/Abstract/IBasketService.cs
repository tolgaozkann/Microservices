using Microservices.Services.Basket.WebAPI.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Basket.WebAPI.Services.Abstract;

public interface IBasketService
{
    Task<ResponseDto<BasketDto>> GetBasket(string userId);
    Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto);
    Task<ResponseDto<bool>> DeleteBasket(string userId);
}