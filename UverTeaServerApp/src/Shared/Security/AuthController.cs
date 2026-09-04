using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UverTeaServerApp.Shared.Security;

/// <summary>
/// Authentication endpoints for issuing JWT access tokens.
/// </summary>
[Route("api/auth")]
[ApiController]
[Tags("Authentication")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Authenticates user credentials and returns a signed JWT access token.
    /// </summary>
    /// <param name="command">User login credentials (username and password)</param>
    /// <response code="200">Login successful, returns JWT token and user details</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
