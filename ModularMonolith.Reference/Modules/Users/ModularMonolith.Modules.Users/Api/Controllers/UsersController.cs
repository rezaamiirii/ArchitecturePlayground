using Microsoft.AspNetCore.Mvc;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Api.Requests;
using ModularMonolith.Modules.Users.Application;

namespace ModularMonolith.Modules.Users.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _usersService.CreateAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _usersService.GetAsync(id, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<UserResponse>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _usersService.SetActiveAsync(id, active: true, cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult<UserResponse>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _usersService.SetActiveAsync(id, active: false, cancellationToken);

        return this.ToActionResult(result);
    }
}
