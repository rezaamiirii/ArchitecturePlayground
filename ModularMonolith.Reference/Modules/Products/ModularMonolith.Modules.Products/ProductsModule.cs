using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Modules.Products.Application;
using ModularMonolith.Modules.Products.Contracts;
using ModularMonolith.Modules.Products.Infrastructure;

namespace ModularMonolith.Modules.Products;

public static class ProductsModule
{
    public static IMvcBuilder AddProductsModule(
        this IMvcBuilder mvcBuilder,
        IConfiguration configuration)
    {
        mvcBuilder.Services.AddDbContext<ProductsDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Products") ?? "Data Source=modular-monolith.db"));

        mvcBuilder.Services.AddScoped<ProductsService>();
        mvcBuilder.Services.AddScoped<IProductsModule>(serviceProvider =>
            serviceProvider.GetRequiredService<ProductsService>());

        mvcBuilder.AddApplicationPart(typeof(ProductsModule).Assembly);

        return mvcBuilder;
    }

    public static async Task SeedProductsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Products.AnyAsync())
        {
            return;
        }

        var keyboard = Domain.Product.Create("Keyboard", 99, 10);
        var mouse = Domain.Product.Create("Mouse", 49, 20);
        var monitor = Domain.Product.Create("Monitor", 299, 5);
        var inactiveCable = Domain.Product.Create("Retired Cable", 9, 100);
        inactiveCable.Deactivate();

        dbContext.Products.AddRange(keyboard, mouse, monitor, inactiveCable);
        await dbContext.SaveChangesAsync();
    }
}
