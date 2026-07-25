using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users.Application.Features.GetUserById;

public sealed class Handler
{
    private readonly UsersDbContext _dbContext;

    public Handler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<UserResponse>> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync([query.Id], cancellationToken);

        if (user is null)
        {
            return Result<UserResponse>.Failure(
                "users.not_found",
                "User was not found.",
                statusCode: 404);
        }

        return Result<UserResponse>.Success(UserResponse.FromDomain(user));
    }
}
