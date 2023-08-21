using Microservices.Services.Discount.WebAPI.Services.Abstract;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Discount.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomControllerBase
    {
        #region fields
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;
        #endregion
        
        #region ctor
        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }
        #endregion

        #region methods

        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResultInstance(await _discountService.GetAll());
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResultInstance(await _discountService.GetById(id));

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = _sharedIdentityService.GetUserId;
            var discount = await _discountService.GetByCodeAndUserId(userId, code);

            return CreateActionResultInstance(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount) => CreateActionResultInstance(await _discountService.Save(discount));

        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount) =>
            CreateActionResultInstance(await _discountService.Update(discount));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            CreateActionResultInstance(await _discountService.DeleteById(id));

        #endregion

    }
}
