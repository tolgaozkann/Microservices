using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Order.Application.Commands;

public class CreateOrderCommand : IRequest<ResponseDto<CreatedOrderDto>>
{
    public string UserId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
    public AddressDto Address { get; set; }
}