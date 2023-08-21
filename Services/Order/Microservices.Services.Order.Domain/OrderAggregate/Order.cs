using Microservices.Services.Order.Domain.Core;

namespace Microservices.Services.Order.Domain.OrderAggregate;

public class Order : EntityBase, IAggregateRoot
{
    public DateTime CreatedTime { get; set; }
    public Address Address { get; set; }
    public string BuyerId { get; set; }

    private readonly List<OrderItem> _orderItems;
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;


    public Order(string buyerId, Address address)
    {
        _orderItems = new List<OrderItem>();
        CreatedTime = DateTime.Now;
        BuyerId = buyerId;
        Address = address;
    }

    public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
    {
        var existProduct = _orderItems.Any(x => x.ProductId == productId);
        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
            _orderItems.Add(newOrderItem);
        }
        
    }

    public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
}