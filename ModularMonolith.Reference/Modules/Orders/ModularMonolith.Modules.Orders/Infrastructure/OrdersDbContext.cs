using Microsoft.EntityFrameworkCore;
using ModularMonolith.Modules.Orders.Domain;

namespace ModularMonolith.Modules.Orders.Infrastructure;

public sealed class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    internal DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();

        order.ToTable("Orders");
        order.HasKey(entity => entity.Id);
        order.Property(entity => entity.Total).HasPrecision(18, 2);
        order
            .HasMany(entity => entity.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        var orderItem = modelBuilder.Entity<OrderItem>();

        orderItem.ToTable("OrderItems");
        orderItem.HasKey(entity => entity.Id);
        orderItem.Property(entity => entity.ProductName).HasMaxLength(200);
        orderItem.Property(entity => entity.UnitPrice).HasPrecision(18, 2);
        orderItem.Property(entity => entity.LineTotal).HasPrecision(18, 2);
    }
}
