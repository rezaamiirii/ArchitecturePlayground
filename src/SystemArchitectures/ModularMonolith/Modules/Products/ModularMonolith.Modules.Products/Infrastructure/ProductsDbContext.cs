using Microsoft.EntityFrameworkCore;
using ModularMonolith.Modules.Products.Domain;

namespace ModularMonolith.Modules.Products.Infrastructure;

public sealed class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
        : base(options)
    {
    }

    internal DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var product = modelBuilder.Entity<Product>();

        product.ToTable("Products");
        product.HasKey(entity => entity.Id);
        product.Property(entity => entity.Name).HasMaxLength(200);
        product.Property(entity => entity.Price).HasPrecision(18, 2);
    }
}
