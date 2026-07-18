namespace ModularMonolith.Modules.Users.Application;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc);
