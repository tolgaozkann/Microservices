using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Order.Application.Queries;

public class GetOrdersByUserIdQuery : IRequest<ResponseDto<List<OrderDto>>>
{
    public string UserId { get; set; }
}