﻿namespace Microservices.Services.Order.Application.Dtos;

public class OrderItemDto
{
    #region Properties

    public int Id { get; set; }
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public string PictureUrl { get; set; }
    public decimal Price { get; set; }

    #endregion
}