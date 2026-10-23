using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;
using UverTeaServerApp.src.Feature.GreenLeafModule.Queries.GetGreenLeafYieldAnalysis;
using UverTeaServerApp.src.Feature.GreenLeafModule.Queries.GetProcessingPriority;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Controllers;

[ApiController]
[Route("api/green-leaf/processing")]
[Tags("Green Leaf Processing")]
[Authorize]
public class GreenLeafProcessingController : ControllerBase
{
    private readonly IMediator _mediator;

    public GreenLeafProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves eligible green leaf batches ranked by processing priority.
    /// </summary>
    /// <response code="200">
    /// Returns the ranked processing queue.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("priority")]
    [ProducesResponseType(
        typeof(IReadOnlyList<ProcessingPriorityResult>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<ProcessingPriorityResult>>>
        GetProcessingPriority(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetProcessingPriorityQuery(),
            cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves green leaf yield analysis with optional date and area filters.
    /// </summary>
    /// <param name="fromDate">Inclusive start date.</param>
    /// <param name="toDate">Inclusive end date.</param>
    /// <param name="areaId">Optional area ID.</param>
    /// <param name="cancellationToken">Request cancellation token.</param>
    /// <response code="200">Yield analysis retrieved successfully.</response>
    /// <response code="400">Invalid query parameters.</response>
    /// <response code="401">Unauthorized - valid JWT required.</response>
    [HttpGet("yield-analysis")]
    [ProducesResponseType(
        typeof(GreenLeafYieldAnalysisResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GreenLeafYieldAnalysisResponse>>
        GetYieldAnalysis(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? areaId,
            CancellationToken cancellationToken)
    {
        var query = new GetGreenLeafYieldAnalysisQuery(
            fromDate,
            toDate,
            areaId);

        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}