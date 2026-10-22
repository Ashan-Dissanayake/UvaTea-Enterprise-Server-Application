using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;
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
}