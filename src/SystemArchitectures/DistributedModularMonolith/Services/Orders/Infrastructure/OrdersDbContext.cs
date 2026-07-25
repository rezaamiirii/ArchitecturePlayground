using DistributedMonolith.Orders.Domain;using Microsoft.EntityFrameworkCore;
namespace DistributedMonolith.Orders.Infrastructure;
public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options):DbContext(options){public DbSet<Order> Orders=>Set<Order>();}
