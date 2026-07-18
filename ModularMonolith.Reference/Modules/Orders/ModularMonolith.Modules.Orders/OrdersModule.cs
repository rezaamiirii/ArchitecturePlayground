using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Modules.Orders.Application;
using ModularMonolith.Modules.Orders.Infrastructure;

namespace ModularMonolith.Modules.Orders;

public static class OrdersModule
{
    public static IMvcBuilder AddOrdersModule(
        this IMvcBuilder mvcBuilder,
        IConfiguration configuration)
    {
        mvcBuilder.Services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Orders") ?? "Data Source=modular-monolith.db"));

        mvcBuilder.Services.AddScoped<OrdersService>();

        mvcBuilder.AddApplicationPart(typeof(OrdersModule).Assembly);

        return mvcBuilder;
    }

    public static async Task EnsureOrdersDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

        await dbContext.Database.EnsureCreatedAsync();
    }
}
