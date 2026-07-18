using Monolith.Api.Exceptions;
using Monolith.Api.Models;
using Monolith.Api.Repositories;

namespace Monolith.Api.Services;

public sealed class UserService(UserRepository users)
{
    public Task<List<User>> ListAsync() => users.ListAsync();
    public async Task<User> GetByIdAsync(int id) => await users.GetByIdAsync(id) ?? throw new NotFoundException($"User {id} was not found.");
    public async Task<User> CreateAsync(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ValidationException("Email is required.");
        email = email.Trim().ToLowerInvariant();
        if (await users.EmailExistsAsync(email)) throw new ConflictException("Email must be unique.");
        var user = new User { FirstName = firstName.Trim(), LastName = lastName.Trim(), Email = email };
        users.Add(user); await users.SaveChangesAsync(); return user;
    }
    public async Task<User> DeactivateAsync(int id)
    {
        var user = await GetByIdAsync(id); user.IsActive = false; await users.SaveChangesAsync(); return user;
    }
}
