using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users.Application.Features.DeactivateUser;

public sealed class Handler
{
    private readonly UsersDbContext _dbContext;

    public Handler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> HandleAsync(Command command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([command.Id], cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(
                "users.not_found",
                "User was not found.",
                statusCode: 404);
        }

        user.Deactivate();
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<UserResponse>.Success(UserResponse.FromDomain(user));
    }
}
