namespace ModularMonolith.Modules.Users.Contracts;

public sealed record UserOrderInfo(
    Guid Id,
    string FullName,
    bool IsActive);
