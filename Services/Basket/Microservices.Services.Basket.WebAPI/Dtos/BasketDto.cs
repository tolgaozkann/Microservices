namespace Microservices.Services.Basket.WebAPI.Dtos;

public class BasketDto
{
   public string UserId { get; set; } 
   public string DiscountCode { get; set;}
   public ICollection<BasketItemDto> BasketItems { get; set; }
   public decimal TotalPrice
   {
       get => BasketItems.Sum(b => b.Price * b.Quantity);
   }
}