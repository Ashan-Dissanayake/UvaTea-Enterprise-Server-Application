using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.AreaModule.Commands.DeactivatePlantingConfiguration;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Controllers;

[ApiController]
[Route("api/planting-configurations")]
[Tags("Planting Configurations")]
public class PlantingConfigurationController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlantingConfigurationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Deactivates an active planting configuration.
    /// </summary>
    /// <param name="id">Planting Configuration ID</param>
    /// <response code="200">
    /// Planting configuration successfully deactivated
    /// </response>
    /// <response code="400">Invalid ID or validation error</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">
    /// Planting configuration not found
    /// </response>
    [Authorize]
    [HttpPut("{id}/deactivate")]
    [ProducesResponseType(
        typeof(PlantingConfigurationDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PlantingConfigurationDto>>
        Deactivate(int id)
    {
        var command = new DeactivatePlantingConfigurationCommand(id);

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}