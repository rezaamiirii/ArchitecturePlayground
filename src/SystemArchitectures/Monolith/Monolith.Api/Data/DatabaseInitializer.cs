using Monolith.Api.Models;

namespace Monolith.Api.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (dbContext.Users.Any() || dbContext.Products.Any()) return;

        dbContext.Users.AddRange(
            new User { FirstName = "Ava", LastName = "Johnson", Email = "ava@example.com" },
            new User { FirstName = "Noah", LastName = "Smith", Email = "noah@example.com" });

        dbContext.Products.AddRange(
            new Product { Name = "Laptop", Description = "Developer laptop", Price = 1299m, AvailableStock = 10 },
            new Product { Name = "Keyboard", Description = "Mechanical keyboard", Price = 149m, AvailableStock = 25 },
            new Product { Name = "Mouse", Description = "Wireless mouse", Price = 79m, AvailableStock = 40 },
            new Product { Name = "Discontinued Monitor", Description = "Inactive sample product", Price = 299m, AvailableStock = 5, IsActive = false });

        await dbContext.SaveChangesAsync();
    }
}
