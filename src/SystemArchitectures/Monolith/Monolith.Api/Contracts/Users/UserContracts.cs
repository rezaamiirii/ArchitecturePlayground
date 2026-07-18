namespace Monolith.Api.Contracts.Users;

public sealed record CreateUserRequest(string FirstName, string LastName, string Email);
public sealed record UserResponse(int Id, string FirstName, string LastName, string Email, bool IsActive, DateTime CreatedAtUtc);
