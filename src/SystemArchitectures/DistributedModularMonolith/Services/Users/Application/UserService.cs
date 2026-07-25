using DistributedMonolith.SharedContracts;
using DistributedMonolith.Users.Domain;
using DistributedMonolith.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;
namespace DistributedMonolith.Users.Application;
public sealed class UserService(UsersDbContext db) {
 public async Task<UserResponse> Create(CreateUserRequest r) { var user = new User { Id=Guid.NewGuid(), Name=r.Name, Email=r.Email }; db.Users.Add(user); await db.SaveChangesAsync(); return Map(user); }
 public async Task<UserResponse?> Get(Guid id) { var u=await db.Users.AsNoTracking().SingleOrDefaultAsync(x=>x.Id==id); return u is null?null:Map(u); }
 public async Task<UserResponse?> Update(Guid id, UpdateUserRequest r) { var u=await db.Users.FindAsync(id); if(u is null)return null; u.Name=r.Name; u.Email=r.Email; await db.SaveChangesAsync(); return Map(u); }
 public Task<bool> Exists(Guid id) => db.Users.AnyAsync(x=>x.Id==id);
 private static UserResponse Map(User u)=>new(u.Id,u.Name,u.Email);
}
