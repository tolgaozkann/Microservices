using Microsoft.EntityFrameworkCore;

namespace Mivroservices.Services.Order.Infastructure.EfCore;

public class OrderDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "Ordering";

    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
        
    }

    public DbSet<Microservices.Services.Order.Domain.OrderAggregate.Order> Orders { get; set; }
    public DbSet<Microservices.Services.Order.Domain.OrderAggregate.OrderItem> OrderItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Microservices.Services.Order.Domain.OrderAggregate.Order>()
            .ToTable("Orders", DEFAULT_SCHEMA);

        modelBuilder.Entity<Microservices.Services.Order.Domain.OrderAggregate.OrderItem>()
            .ToTable("OrderItems", DEFAULT_SCHEMA);

        modelBuilder.Entity<Microservices.Services.Order.Domain.OrderAggregate.OrderItem>()
            .Property(x => x.Price).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Microservices.Services.Order.Domain.OrderAggregate.Order>()
            .OwnsOne(x => x.Address).WithOwner();

        base.OnModelCreating(modelBuilder);
    }
}