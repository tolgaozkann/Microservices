﻿namespace Microservices.Services.Discount.WebAPI.Models;


[Dapper.Contrib.Extensions.Table("discount")]
public class Discount
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int Rate { get; set; }
    public string Code { get; set; }
    public DateTime CreatedAt { get; set; }
}