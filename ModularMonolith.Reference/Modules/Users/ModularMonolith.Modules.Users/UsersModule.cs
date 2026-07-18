using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularMonolith.Modules.Users.Application;
using ModularMonolith.Modules.Users.Contracts;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users;

public static class UsersModule
{
    public static IMvcBuilder AddUsersModule(
        this IMvcBuilder mvcBuilder,
        IConfiguration configuration)
    {
        mvcBuilder.Services.AddDbContext<UsersDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Users") ?? "Data Source=modular-monolith.db"));

        mvcBuilder.Services.AddScoped<UsersService>();
        mvcBuilder.Services.AddScoped<IUsersModule>(serviceProvider =>
            serviceProvider.GetRequiredService<UsersService>());

        mvcBuilder.AddApplicationPart(typeof(UsersModule).Assembly);

        return mvcBuilder;
    }

    public static async Task SeedUsersAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var activeUser = Domain.User.Create("Ada", "Lovelace", "ada@example.com");
        var secondActiveUser = Domain.User.Create("Grace", "Hopper", "grace@example.com");
        var inactiveUser = Domain.User.Create("Inactive", "User", "inactive@example.com");
        inactiveUser.Deactivate();

        dbContext.Users.AddRange(activeUser, secondActiveUser, inactiveUser);
        await dbContext.SaveChangesAsync();
    }
}
