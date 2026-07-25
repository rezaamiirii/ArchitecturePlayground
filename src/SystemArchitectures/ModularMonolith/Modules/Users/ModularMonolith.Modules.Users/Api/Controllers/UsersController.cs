using Microsoft.AspNetCore.Mvc;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Users.Api.Requests;
using ModularMonolith.Modules.Users.Application;

namespace ModularMonolith.Modules.Users.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly Application.Features.CreateUser.Handler _createUserHandler;
    private readonly Application.Features.GetUserById.Handler _getUserByIdHandler;
    private readonly Application.Features.ActivateUser.Handler _activateUserHandler;
    private readonly Application.Features.DeactivateUser.Handler _deactivateUserHandler;

    public UsersController(
        Application.Features.CreateUser.Handler createUserHandler,
        Application.Features.GetUserById.Handler getUserByIdHandler,
        Application.Features.ActivateUser.Handler activateUserHandler,
        Application.Features.DeactivateUser.Handler deactivateUserHandler)
    {
        _createUserHandler = createUserHandler;
        _getUserByIdHandler = getUserByIdHandler;
        _activateUserHandler = activateUserHandler;
        _deactivateUserHandler = deactivateUserHandler;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _createUserHandler.HandleAsync(
            new Application.Features.CreateUser.Command(
                request.FirstName,
                request.LastName,
                request.Email),
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Get(Guid id, CancellationToken cancellationToken)
    {
        var result = await _getUserByIdHandler.HandleAsync(
            new Application.Features.GetUserById.Query(id),
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    public async Task<ActionResult<UserResponse>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _activateUserHandler.HandleAsync(
            new Application.Features.ActivateUser.Command(id),
            cancellationToken);

        return this.ToActionResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    public async Task<ActionResult<UserResponse>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deactivateUserHandler.HandleAsync(
            new Application.Features.DeactivateUser.Command(id),
            cancellationToken);

        return this.ToActionResult(result);
    }
}
