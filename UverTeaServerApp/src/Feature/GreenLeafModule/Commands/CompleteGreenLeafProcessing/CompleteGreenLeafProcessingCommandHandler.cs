
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;

public sealed class CompleteGreenLeafProcessingCommandHandler
    : IRequestHandler<
        CompleteGreenLeafProcessingCommand,
        CompleteGreenLeafProcessingResponse>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteGreenLeafProcessingCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<CompleteGreenLeafProcessingResponse> Handle(
        CompleteGreenLeafProcessingCommand request,
        CancellationToken cancellationToken)
    {
        var batch = await _context.Set<Greenleafbatch>()
            .Include(x => x.LeafBatchStatus)
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
                "PROCESSING",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only green leaf batches in PROCESSING status can be completed.");
        }

        if (request.ProcessedWeight > batch.Weight)
        {
            throw new InvalidOperationException(
                "Processed weight cannot exceed the received green leaf weight.");
        }

        var alreadyCompleted = await _context
            .Set<Greenleafprocessingrecord>()
            .AnyAsync(
                x => x.GreenLeafBatchId == batch.Id,
                cancellationToken);

        if (alreadyCompleted)
        {
            throw new InvalidOperationException(
                "This green leaf batch has already been completed.");
        }

        var processedStatus = await _context.Set<Leafbatchstatus>()
            .SingleOrDefaultAsync(
                x => x.Name == "PROCESSED",
                cancellationToken);

        if (processedStatus is null)
        {
            throw new InvalidOperationException(
                "The PROCESSED status is not configured.");
        }

        var completedAt = DateTime.UtcNow;

        var processingRecord = new Greenleafprocessingrecord
        {
            GreenLeafBatchId = batch.Id,
            ProcessedWeight = request.ProcessedWeight,
            ProcessingNotes = request.ProcessingNotes,
            CompletedAt = completedAt
        };

        batch.LeafBatchStatusId = processedStatus.Id;
        batch.LeafBatchStatus = processedStatus;
        batch.UpdatedAt = completedAt;

        _context.Set<Greenleafprocessingrecord>()
            .Add(processingRecord);

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
        catch (DbUpdateException)
        {
            // The unique GreenLeafBatchId index is the final safeguard
            // against duplicate processing records.
            throw new InvalidOperationException(
                "The processing result could not be saved. " +
                "The batch may already have been completed.");
        }

        return new CompleteGreenLeafProcessingResponse(
            batch.Id,
            batch.BatchNumber,
            processingRecord.ProcessedWeight,
            processingRecord.ProcessingNotes,
            processingRecord.CompletedAt,
            processedStatus.Name!);
    }
}
