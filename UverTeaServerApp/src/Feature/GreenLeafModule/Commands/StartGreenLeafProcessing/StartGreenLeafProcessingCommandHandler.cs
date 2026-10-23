using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .StartGreenLeafProcessing;

public sealed class StartGreenLeafProcessingCommandHandler
    : IRequestHandler<
        StartGreenLeafProcessingCommand,
        StartGreenLeafProcessingResponse>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public StartGreenLeafProcessingCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<StartGreenLeafProcessingResponse> Handle(
        StartGreenLeafProcessingCommand request,
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
                "QUEUED",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only queued green leaf batches can start processing.");
        }

        if (batch.RiskAssessment is null)
        {
            throw new InvalidOperationException(
                "A risk assessment is required before processing.");
        }

        var processingStatus = await _context.Set<Leafbatchstatus>()
            .SingleOrDefaultAsync(
                x => x.Name == "PROCESSING",
                cancellationToken);

        if (processingStatus is null)
        {
            throw new InvalidOperationException(
                "The PROCESSING status is not configured.");
        }

        batch.LeafBatchStatusId = processingStatus.Id;
        batch.LeafBatchStatus = processingStatus;
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

        return new StartGreenLeafProcessingResponse(
            batch.Id,
            batch.BatchNumber,
            processingStatus.Name!);
    }
}