namespace ModularMonolith.Modules.Users.Api.Requests;

public sealed record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email);
