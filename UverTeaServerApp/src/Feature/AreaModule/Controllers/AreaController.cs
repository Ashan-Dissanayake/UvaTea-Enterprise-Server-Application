using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.AreaModule.Commands.CreateArea;
using UverTeaServerApp.AreaModule.Commands.UpdateArea;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Queries.GetAllAreas;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.AreaModule.Queries.SeachAreas;
using UverTeaServerApp.AreaModule.Commands.ChangeGrowthStage;

namespace UverTeaServerApp.src.Feature.AreaModule.Controllers;

/// <summary>
/// Endpoints for managing area registrations, updates, and searches.
/// </summary>
[ApiController]
[Route("api/areas")]
[Tags("Areas")]
public class AreaController : ControllerBase
{
    private readonly IMediator _mediator;

    public AreaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves a paginated list of all active areas.
    /// </summary>
    /// <param name="paginationParams">
    /// Pagination, searching, and sorting criteria
    /// </param>
    /// <response code="200">
    /// Returns the paginated area records
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required
    /// </response>
    [Authorize]
    [HttpGet]
    [ProducesResponseType(
        typeof(PagedResult<AreaDetailResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<AreaDetailResponseDto>>>
        GetAllAreas(
            [FromQuery] PaginationParams paginationParams)
    {
        var result =
            await _mediator.Send(
                new GetAllAreasQuery(paginationParams));

        return Ok(result);
    }

    /// <summary>
    /// Searches areas using dynamic field criteria.
    /// </summary>
    /// <param name="paramsDict">
    /// Key-value filter parameters
    /// </param>
    /// <param name="paginationParams">
    /// Pagination criteria
    /// </param>
    /// <response code="200">
    /// Returns matching areas
    /// </response>
    /// <response code="401">
    /// Unauthorized
    /// </response>
    [Authorize]
    [HttpGet("search")]
    [ProducesResponseType(
        typeof(PagedResult<AreaDetailResponseDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<AreaDetailResponseDto>>>
        SearchAreas(
            [FromQuery] Dictionary<string, string?> paramsDict,
            [FromQuery] PaginationParams paginationParams)
    {
        var result =
            await _mediator.Send(
                new SearchAreasQuery(
                    paramsDict,
                    paginationParams));

        return Ok(result);
    }

    /// <summary>
    /// Registers a new area.
    /// </summary>
    /// <param name="command">
    /// Area details and planting configuration
    /// </param>
    /// <response code="201">
    /// Area successfully created
    /// </response>
    /// <response code="400">
    /// Validation error
    /// </response>
    /// <response code="401">
    /// Unauthorized
    /// </response>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(
        typeof(AreaDetailResponseDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AreaDetailResponseDto>>
        CreateArea(
            [FromBody] CreateAreaCommand command)
    {
        var result = await _mediator.Send(command);

        return StatusCode(
            StatusCodes.Status201Created,
            result);
    }

    /// <summary>
    /// Updates an existing active area.
    /// </summary>
    /// <param name="id">
    /// Area ID
    /// </param>
    /// <param name="command">
    /// Updated area payload
    /// </param>
    /// <response code="200">
    /// Area successfully updated
    /// </response>
    /// <response code="400">
    /// ID mismatch or validation error
    /// </response>
    /// <response code="401">
    /// Unauthorized
    /// </response>
    /// <response code="404">
    /// Area not found
    /// </response>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(
        typeof(AreaDetailResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AreaDetailResponseDto>>
        UpdateArea(
            int id,
            [FromBody] UpdateAreaCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "ID mismatch between URL and body.");
        }

        var result = await _mediator.Send(command);

        return Ok(result);
    }


    /// <summary>
    /// Changes the growth stage of an active area.
    /// </summary>
    /// <param name="id">Area ID</param>
    /// <param name="command">Growth stage transition details</param>
    /// <response code="200">
    /// Growth stage successfully changed
    /// </response>
    /// <response code="400">
    /// Invalid ID, validation error, or invalid growth stage transition
    /// </response>
    /// <response code="401">
    /// Unauthorized
    /// </response>
    /// <response code="404">
    /// Area or growth stage not found
    /// </response>
    [Authorize]
    [HttpPut("{id}/growth-stage")]
    [ProducesResponseType( typeof(AreaDetailResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AreaDetailResponseDto>>
        ChangeGrowthStage(int id, [FromBody] ChangeGrowthStageCommand command)
    {
        if (id != command.AreaId)
        {
            return BadRequest(
                "ID mismatch between URL and body.");
        }

        var result = await _mediator.Send(command);

        return Ok(result);
    }
}