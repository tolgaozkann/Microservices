using MediatR;
using Microservices.Services.Order.Application.Commands;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Application.Mapping;
using Microservices.Services.Order.Domain.OrderAggregate;
using Microservices.Services.Order.Infrastructure.EfCore;
using Microservices.Shared.Dtos;

namespace Microservices.Services.Order.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ResponseDto<CreatedOrderDto>>
{
    private readonly OrderDbContext _context;

    public CreateOrderCommandHandler(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseDto<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street,
            request.Address.ZipCode, request.Address.Line);

        Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.UserId, newAddress);

        request.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _context.Orders.AddAsync(newOrder);

        var result = await _context.SaveChangesAsync();

        return ResponseDto<CreatedOrderDto>.Success(new CreatedOrderDto(){OrderId = newOrder.Id},200);

    }
}