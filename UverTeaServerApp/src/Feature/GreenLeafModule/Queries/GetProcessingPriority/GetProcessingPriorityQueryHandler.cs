using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries.GetProcessingPriority;

public sealed class GetProcessingPriorityQueryHandler
    : IRequestHandler<
        GetProcessingPriorityQuery,
        IReadOnlyList<ProcessingPriorityResult>>
{
    private const string QueuedStatus = "QUEUED";

    private readonly UvaTeaDbContext _context;
    private readonly ProcessingPriorityEngine _priorityEngine;

    public GetProcessingPriorityQueryHandler(
        UvaTeaDbContext context,
        ProcessingPriorityEngine priorityEngine)
    {
        _context = context;
        _priorityEngine = priorityEngine;
    }

    public async Task<IReadOnlyList<ProcessingPriorityResult>> Handle(
        GetProcessingPriorityQuery request,
        CancellationToken cancellationToken)
    {
        var eligibleBatches = await _context.Greenleafbatches
            .AsNoTracking()
            .Include(batch => batch.LeafBatchStatus)
            .Include(batch => batch.RiskAssessment)
            .Where(batch =>
            batch.LeafBatchStatus.Name == QueuedStatus &&
            batch.RiskAssessment != null)
            .ToListAsync(cancellationToken);

        return _priorityEngine.Rank(
            eligibleBatches,
            DateTime.UtcNow);
    }
}