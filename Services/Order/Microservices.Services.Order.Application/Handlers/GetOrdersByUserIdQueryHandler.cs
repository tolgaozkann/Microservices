using MediatR;
using Microservices.Services.Order.Application.Dtos;
using Microservices.Services.Order.Application.Mapping;
using Microservices.Services.Order.Application.Queries;
using Microservices.Services.Order.Infrastructure.EfCore;
using Microservices.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Services.Order.Application.Handlers;

public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery,ResponseDto<List<OrderDto>>>
{
    private readonly OrderDbContext _context;

    public GetOrdersByUserIdQueryHandler(OrderDbContext context)
    {
        _context = context;
    }


    public async Task<ResponseDto<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var orders = await _context
            .Orders
            .Include(x => x.OrderItems)
            .Where(o => o.BuyerId == request.UserId)
            .ToListAsync();

        return !orders.Any() ? ResponseDto<List<OrderDto>>.Success(new List<OrderDto>(), 200) : 
            ResponseDto<List<OrderDto>>.Success(ObjectMapper.Mapper.Map<List<OrderDto>>(orders), 200);
       
    }
}