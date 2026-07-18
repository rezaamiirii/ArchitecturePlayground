namespace ModularMonolith.Modules.Users.Contracts;
public interface IUsersModule { Task<UserOrderInfo?> GetUserForOrderAsync(Guid userId, CancellationToken cancellationToken); }
public sealed record UserOrderInfo(Guid Id, string FullName, bool IsActive);
