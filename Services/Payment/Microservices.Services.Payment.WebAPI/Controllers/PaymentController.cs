using Microservices.Shared.ControllerBase;
using Microservices.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Payment.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : CustomControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceivePayment() => CreateActionResultInstance(ResponseDto<NoContent>.Success(200));
        
    }
}
