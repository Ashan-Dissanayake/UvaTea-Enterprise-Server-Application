using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.UserModule.Commands.CreateUser;
using UverTeaServerApp.src.Feature.UserModule.Commands.DeleteUser;
using UverTeaServerApp.src.Feature.UserModule.Commands.UpdateUser;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;
using UverTeaServerApp.src.Feature.UserModule.Queries.GetUserById;
using UverTeaServerApp.src.Feature.UserModule.Queries.GetUsers;

namespace UverTeaServerApp.src.Feature.UserModule.Controllers;

/// <summary>
/// Endpoints for managing user accounts, credential assignments, and RBAC roles.
/// </summary>
[ApiController]
[Route("api/users")]
[Tags("Users & RBAC")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns paginated users with their Role and Employee details.
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(PagedResult<UserDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<UserDetailResponseDto>>> GetUsers(
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new GetUsersQuery(paginationParams));
        return Ok(result);
    }

    /// <summary>
    /// Returns a single user by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UserDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailResponseDto>> GetUserById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Registers a new application user (Requires Admin role).
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDetailResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UserDetailResponseDto>> RegisterUser(
        [FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Updates an existing user's details (username, role, status, optional password re-hash).
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(UserDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDetailResponseDto>> UpdateUser(
        int id, [FromBody] UpdateUserCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch between URL and body.");
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}
