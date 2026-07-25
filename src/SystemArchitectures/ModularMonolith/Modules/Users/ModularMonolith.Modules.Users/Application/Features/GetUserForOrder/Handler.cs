using Microsoft.EntityFrameworkCore;
using ModularMonolith.Modules.Users.Contracts;
using ModularMonolith.Modules.Users.Infrastructure;

namespace ModularMonolith.Modules.Users.Application.Features.GetUserForOrder;

public sealed class Handler
{
    private readonly UsersDbContext _dbContext;

    public Handler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserOrderInfo?> HandleAsync(Query query, CancellationToken cancellationToken)
    {
        return await _dbContext.Users
            .Where(user => user.Id == query.UserId)
            .Select(user => new UserOrderInfo(
                user.Id,
                user.FirstName + " " + user.LastName,
                user.IsActive))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
