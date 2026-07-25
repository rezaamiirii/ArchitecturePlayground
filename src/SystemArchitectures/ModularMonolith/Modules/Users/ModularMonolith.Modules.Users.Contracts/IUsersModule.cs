namespace ModularMonolith.Modules.Users.Contracts;

public interface IUsersModule
{
    Task<UserOrderInfo?> GetUserForOrderAsync(
        Guid userId,
        CancellationToken cancellationToken);
}
