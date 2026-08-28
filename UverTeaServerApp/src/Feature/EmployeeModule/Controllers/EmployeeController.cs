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

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PagedResult<EmployeeDetailResponseDto>>> GetAllEmployees(
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new GetAllEmployeesQuery(paginationParams));
        return Ok(result);
    }

    [Authorize]
    [HttpGet("search")]
    public async Task<ActionResult<PagedResult<EmployeeDetailResponseDto>>> SearchEmployees(
        [FromQuery] Dictionary<string, string?> paramsDict,
        [FromQuery] PaginationParams paginationParams)
    {
        var result = await _mediator.Send(new SearchEmployeesQuery(paramsDict, paginationParams));
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPost]
    public async Task<ActionResult<EmployeeDetailResponseDto>> CreateEmployee(
        [FromBody] CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(201, result);
    }

    [Authorize(Roles = "Admin,Manager")]
    [HttpPut("{id}")]
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

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}