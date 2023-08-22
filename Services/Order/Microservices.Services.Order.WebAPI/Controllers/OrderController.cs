using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Application.Queries;
using Microservices.Shared.ControllerBase;
using Microservices.Shared.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Services.Order.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : CustomControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            _mediator = mediator;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrdersByUserIdQuery()
            {
                UserId = _sharedIdentityService.GetUserId
            });

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand command)
        {
            var response = await _mediator.Send(command);

            return CreateActionResultInstance(response);
        }
    }
}
