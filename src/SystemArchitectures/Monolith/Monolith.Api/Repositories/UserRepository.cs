using Microsoft.EntityFrameworkCore;
using Monolith.Api.Data;
using Monolith.Api.Models;

namespace Monolith.Api.Repositories;

public sealed class UserRepository(AppDbContext dbContext)
{
    public AppDbContext DbContext => dbContext;
    public Task<List<User>> ListAsync() => dbContext.Users.OrderBy(x => x.Id).ToListAsync();
    public Task<User?> GetByIdAsync(int id) => dbContext.Users.FindAsync(id).AsTask();
    public Task<bool> EmailExistsAsync(string email) => dbContext.Users.AnyAsync(x => x.Email == email);
    public void Add(User user) => dbContext.Users.Add(user);
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
