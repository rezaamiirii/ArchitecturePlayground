using ModularMonolith.Modules.Users.Contracts;

namespace ModularMonolith.Modules.Users.Application;

public sealed class UsersModuleFacade : IUsersModule
{
    private readonly Features.GetUserForOrder.Handler _getUserForOrderHandler;

    public UsersModuleFacade(Features.GetUserForOrder.Handler getUserForOrderHandler)
    {
        _getUserForOrderHandler = getUserForOrderHandler;
    }

    public async Task<UserOrderInfo?> GetUserForOrderAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _getUserForOrderHandler.HandleAsync(
            new Features.GetUserForOrder.Query(userId),
            cancellationToken);
    }
}
