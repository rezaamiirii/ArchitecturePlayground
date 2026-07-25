using DistributedMonolith.Products.Domain; using Microsoft.EntityFrameworkCore;
namespace DistributedMonolith.Products.Infrastructure;
public sealed class ProductsDbContext(DbContextOptions<ProductsDbContext> options):DbContext(options){public DbSet<Product> Products=>Set<Product>();}
