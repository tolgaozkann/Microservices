using Microservices.Services.Basket.WebAPI.Dtos;
using Microservices.Services.Basket.WebAPI.Services.Abstract;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Basket.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket() =>
             CreateActionResultInstance(await _basketService.GetBasket(_sharedIdentityService.GetUserId));

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket(BasketDto basketDto) =>
            CreateActionResultInstance(await _basketService.SaveOrUpdate(basketDto));

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket() =>
            CreateActionResultInstance(await _basketService.DeleteBasket(_sharedIdentityService.GetUserId));

    }
}
