using Microsoft.AspNetCore.Mvc;
using Monolith.Api.Contracts.Users;
using Monolith.Api.Models;
using Monolith.Api.Services;

namespace Monolith.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController(UserService users) : ControllerBase
{
    [HttpGet]
    public async Task<IReadOnlyList<UserResponse>> List() => (await users.ListAsync()).Select(ToResponse).ToList();
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResponse>> Get(int id) => ToResponse(await users.GetByIdAsync(id));
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request)
    {
        var user = await users.CreateAsync(request.FirstName, request.LastName, request.Email);
        return CreatedAtAction(nameof(Get), new { id = user.Id }, ToResponse(user));
    }
    [HttpPatch("{id:int}/deactivate")]
    public async Task<UserResponse> Deactivate(int id) => ToResponse(await users.DeactivateAsync(id));
    private static UserResponse ToResponse(User user) => new(user.Id, user.FirstName, user.LastName, user.Email, user.IsActive, user.CreatedAtUtc);
}
