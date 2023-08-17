namespace Microservices.Services.Basket.WebAPI.Dtos;

public class BasketItemDto
{
    public int Quantity { get; set; }
    public string CourseId { get; set; }
    public string CourseName { get; set; }
    public decimal Price { get; set; }
}