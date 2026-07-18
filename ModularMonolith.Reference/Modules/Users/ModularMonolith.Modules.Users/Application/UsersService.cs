using Microsoft.EntityFrameworkCore;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Contracts;
using ModularMonolith.Modules.Users.Domain;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users.Application;

public sealed class UsersService : IUsersModule
{
    private readonly UsersDbContext _dbContext;

    public UsersService(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> CreateAsync(
        string firstName,
        string lastName,
        string email,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = User.Create(firstName, lastName, email);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<UserResponse>.Success(
                Map(user),
                statusCode: 201);
        }
        catch (Exception exception) when (exception is ArgumentException or DbUpdateException)
        {
            return Result<UserResponse>.Failure(
                "users.invalid",
                exception.Message,
                statusCode: 400);
        }
    }

    public async Task<Result<UserResponse>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(
                "users.not_found",
                "User was not found.",
                statusCode: 404);
        }

        return Result<UserResponse>.Success(Map(user));
    }

    public async Task<Result<UserResponse>> SetActiveAsync(
        Guid id,
        bool active,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([id], cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(
                "users.not_found",
                "User was not found.",
                statusCode: 404);
        }

        if (active)
        {
            user.Activate();
        }
        else
        {
            user.Deactivate();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<UserResponse>.Success(Map(user));
    }

    public async Task<UserOrderInfo?> GetUserForOrderAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(user => user.Id == userId)
            .Select(user => new UserOrderInfo(
                user.Id,
                user.FirstName + " " + user.LastName,
                user.IsActive))
            .SingleOrDefaultAsync(cancellationToken);
    }

    private static UserResponse Map(User user)
    {
        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.IsActive,
            user.CreatedAtUtc);
    }
}
