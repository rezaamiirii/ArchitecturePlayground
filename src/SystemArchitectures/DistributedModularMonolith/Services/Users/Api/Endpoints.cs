using DistributedMonolith.SharedContracts;
using DistributedMonolith.Users.Application;
namespace DistributedMonolith.Users.Api;
public static class Endpoints { public static void MapUserEndpoints(this WebApplication app) {
 var api=app.MapGroup("/api/users").WithTags("Users");
 api.MapPost("/", async (CreateUserRequest r,UserService s)=>Results.Created("/api/users",await s.Create(r)));
 api.MapGet("/{id:guid}",async(Guid id,UserService s)=>await s.Get(id) is { } u?Results.Ok(u):Results.NotFound());
 api.MapPut("/{id:guid}",async(Guid id,UpdateUserRequest r,UserService s)=>await s.Update(id,r) is { } u?Results.Ok(u):Results.NotFound());
 app.MapGet("/internal/users/{id:guid}/exists",async(Guid id,UserService s)=>Results.Ok(await s.Exists(id))).ExcludeFromDescription();
} }
