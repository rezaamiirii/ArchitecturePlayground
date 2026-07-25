namespace ModularMonolith.Modules.Users.Application.Features.CreateUser;

public sealed record Command(
    string FirstName,
    string LastName,
    string Email);
