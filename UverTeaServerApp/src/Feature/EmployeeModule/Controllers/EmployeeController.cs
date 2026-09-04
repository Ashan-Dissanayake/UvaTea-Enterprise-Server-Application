using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Commands.DeleteEmployee;
using UverTeaServerApp.src.Feature.EmployeeModule.Commands.UpdateEmployee;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Queries.GetAllEmployees;
using UverTeaServerApp.src.Feature.EmployeeModule.Queries.SeachEmployees;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Controllers;

/// <summary>
/// Endpoints for managing employee profiles, registrations, updates, and searches.
/// </summary>
[ApiController]
[Route("api/employees")]
[Tags("Employees")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of all active employees.
    /// </summary>
    /// <param name="paginationParams">Pagination, searching, and sorting criteria</param>
    /// <response code="200">Returns the paginated employee records</response>
    /// <response code="401">Unauthorized - valid JWT required</response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<EmployeeDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<EmployeeDetailResponseDto>>> GetAllEmployees(
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(paginationParams));
        return Ok(result);
    }

    /// <summary>
    /// Searches employees using dynamic field criteria (number, designation, nic, etc.).
    /// </summary>
    /// <param name="paramsDict">Key-value filter parameters</param>
    /// <param name="paginationParams">Pagination criteria</param>
    /// <response code="200">Returns matching employees</response>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<EmployeeDetailResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<EmployeeDetailResponseDto>>> SearchEmployees(
        [FromQuery] Dictionary<string, string?> paramsDict,
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new SearchEmployeesQuery(paramsDict, paginationParams));
        return Ok(result);
    }

    /// <summary>
    /// Registers a new employee and dispatches welcome notifications.
    /// </summary>
    /// <param name="command">Employee details and assignments</param>
    /// <response code="201">Employee successfully created</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="403">Forbidden - requires Admin or Manager role</response>
    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDetailResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<EmployeeDetailResponseDto>> CreateEmployee(
        [FromBody] CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, result);
    }

    /// <summary>
    /// Updates an existing employee profile.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <param name="command">Updated employee payload</param>
    /// <response code="200">Employee successfully updated</response>
    /// <response code="400">ID mismatch or validation error</response>
    /// <response code="404">Employee not found</response>
    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeDetailResponseDto>> UpdateEmployee(
        int id, [FromBody] UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch between URL and body.");
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Soft deletes an employee by ID.
    /// </summary>
    /// <param name="id">Employee ID</param>
    /// <response code="204">Employee successfully deleted</response>
    /// <response code="404">Employee not found</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}