using ModularMonolith.Modules.Users.Domain;

namespace ModularMonolith.Modules.Users.Application;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc)
{
    internal static UserResponse FromDomain(User user)
    {
        return new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.IsActive,
            user.CreatedAtUtc);
    }
}
