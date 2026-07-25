using Microsoft.EntityFrameworkCore;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Domain;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users.Application.Features.CreateUser;

public sealed class Handler
{
    private readonly UsersDbContext _dbContext;

    public Handler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> HandleAsync(
        Command command,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = User.Create(command.FirstName, command.LastName, command.Email);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<UserResponse>.Success(
                UserResponse.FromDomain(user),
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
}
