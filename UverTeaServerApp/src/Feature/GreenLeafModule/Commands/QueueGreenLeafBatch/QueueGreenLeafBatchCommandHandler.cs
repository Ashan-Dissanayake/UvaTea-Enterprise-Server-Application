using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .QueueGreenLeafBatch;

public sealed class QueueGreenLeafBatchCommandHandler
    : IRequestHandler<
        QueueGreenLeafBatchCommand,
        QueueGreenLeafBatchResponse>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public QueueGreenLeafBatchCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<QueueGreenLeafBatchResponse> Handle(
        QueueGreenLeafBatchCommand request,
        CancellationToken cancellationToken)
    {
        var batch = await _context.Set<Greenleafbatch>()
            .Include(x => x.LeafBatchStatus)
            .Include(x => x.RiskAssessment)
            .SingleOrDefaultAsync(
                x => x.Id == request.GreenLeafBatchId,
                cancellationToken);

        if (batch is null)
        {
            throw new KeyNotFoundException(
                $"Green leaf batch {request.GreenLeafBatchId} was not found.");
        }

        if (!string.Equals(
                batch.LeafBatchStatus?.Name,
                "RECEIVED",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only received green leaf batches can be queued.");
        }

        if (batch.RiskAssessment is null)
        {
            throw new InvalidOperationException(
                "A risk assessment is required before queueing.");
        }

        var queuedStatus = await _context.Set<Leafbatchstatus>()
            .SingleOrDefaultAsync(
                x => x.Name == "QUEUED",
                cancellationToken);

        if (queuedStatus is null)
        {
            throw new InvalidOperationException(
                "The QUEUED status is not configured.");
        }

        batch.LeafBatchStatusId = queuedStatus.Id;
        batch.LeafBatchStatus = queuedStatus;
        batch.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new InvalidOperationException(
                "This batch was changed by another request. " +
                "Refresh the batch and try again.");
        }

        return new QueueGreenLeafBatchResponse(
            batch.Id,
            batch.BatchNumber,
            queuedStatus.Name!);
    }
}