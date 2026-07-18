using Microsoft.EntityFrameworkCore;
using Monolith.Api.Data;
using Monolith.Api.Repositories;
using Monolith.Api.Services;

namespace Monolith.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddMonolith(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MonolithDatabase") ?? "Data Source=monolith.db";
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<UserRepository>();
        services.AddScoped<ProductRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<UserService>();
        services.AddScoped<ProductService>();
        services.AddScoped<OrderService>();
        return services;
    }
}
