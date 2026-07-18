using Microsoft.EntityFrameworkCore; using ModularMonolith.BuildingBlocks.Errors; using ModularMonolith.Modules.Users.Contracts; using ModularMonolith.Modules.Users.Domain; using ModularMonolith.Modules.Users.Infrastructure;
namespace ModularMonolith.Modules.Users.Application;
public sealed class UsersService(UsersDbContext db) : IUsersModule
{
 public async Task<Result<UserResponse>> CreateAsync(string first,string last,string email,CancellationToken ct){ try{var u=User.Create(first,last,email); db.Users.Add(u); await db.SaveChangesAsync(ct); return Result<UserResponse>.Success(Map(u),201);} catch(Exception ex) when (ex is ArgumentException or DbUpdateException){ return Result<UserResponse>.Failure("users.invalid",ex.Message,400);} }
 public async Task<Result<UserResponse>> GetAsync(Guid id,CancellationToken ct){var u=await db.Users.FindAsync([id],ct); return u is null?Result<UserResponse>.Failure("users.not_found","User was not found.",404):Result<UserResponse>.Success(Map(u));}
 public async Task<Result<UserResponse>> SetActiveAsync(Guid id,bool active,CancellationToken ct){var u=await db.Users.FindAsync([id],ct); if(u is null)return Result<UserResponse>.Failure("users.not_found","User was not found.",404); if(active)u.Activate();else u.Deactivate(); await db.SaveChangesAsync(ct); return Result<UserResponse>.Success(Map(u));}
 public async Task<UserOrderInfo?> GetUserForOrderAsync(Guid id,CancellationToken ct)=> await db.Users.Where(x=>x.Id==id).Select(x=>new UserOrderInfo(x.Id,x.FirstName+" "+x.LastName,x.IsActive)).SingleOrDefaultAsync(ct);
 static UserResponse Map(User u)=>new(u.Id,u.FirstName,u.LastName,u.Email,u.IsActive,u.CreatedAtUtc);
}
public sealed record UserResponse(Guid Id,string FirstName,string LastName,string Email,bool IsActive,DateTime CreatedAtUtc);
