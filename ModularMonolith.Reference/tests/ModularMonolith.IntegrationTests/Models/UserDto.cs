namespace ModularMonolith.IntegrationTests.Models;

internal sealed record UserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc);
