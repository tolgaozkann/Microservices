using System.Text.Json;
using Microservices.Services.Basket.WebAPI.Dtos;
using Microservices.Services.Basket.WebAPI.Services.Abstract;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Basket.WebAPI.Services.Concrete;

public class BasketService : IBasketService
{
    private readonly RedisService _redisService;

    public BasketService(RedisService redisService)
    {
        _redisService = redisService;
    }

    public async Task<ResponseDto<BasketDto>> GetBasket(string userId)
    {
        var ifExist = await _redisService.GetDb().StringGetAsync(userId);

        if (String.IsNullOrEmpty(ifExist))
            return ResponseDto<BasketDto>.Fail("Basket Not Found", 404);

        return ResponseDto<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(ifExist), 200);

    }

    public async Task<ResponseDto<bool>> SaveOrUpdate(BasketDto basketDto)
    {
        var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));

        return status ? ResponseDto<bool>.Success(200) : ResponseDto<bool>.Fail("Basket could not be updated.", 500);
    }

    public async Task<ResponseDto<bool>> DeleteBasket(string userId)
    {
        var status = await _redisService.GetDb().KeyDeleteAsync(userId);

        return status ? ResponseDto<bool>.Success(204) : ResponseDto<bool>.Fail("Basket not found.", 404);
    }
}