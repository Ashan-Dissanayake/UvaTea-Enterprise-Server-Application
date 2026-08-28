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

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns paginated users with their Role and Employee details.
    /// GET /api/users
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<PagedResult<UserDetailResponseDto>>> GetUsers(
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new GetUsersQuery(paginationParams));
        return Ok(result);
    }

    /// <summary>
    /// Returns a single user by ID.
    /// GET /api/users/{id}
    /// </summary>
    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<ActionResult<UserDetailResponseDto>> GetUserById(int id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(result);
    }

    /// <summary>
    /// Registers a new application user (public — no token required).
    /// POST /api/users/register
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<UserDetailResponseDto>> RegisterUser(
        [FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Updates an existing user's details (username, role, status, optional password re-hash).
    /// PUT /api/users/{id}
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
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
    /// DELETE /api/users/{id}
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return NoContent();
    }
}
