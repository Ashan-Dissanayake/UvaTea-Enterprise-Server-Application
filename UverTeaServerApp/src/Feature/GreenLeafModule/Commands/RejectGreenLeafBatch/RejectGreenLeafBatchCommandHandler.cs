using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .RejectGreenLeafBatch;

public sealed class RejectGreenLeafBatchCommandHandler
    : IRequestHandler<
        RejectGreenLeafBatchCommand,
        RejectGreenLeafBatchResponse>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RejectGreenLeafBatchCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<RejectGreenLeafBatchResponse> Handle(
        RejectGreenLeafBatchCommand request,
        CancellationToken cancellationToken)
    {
        var reason = request.RejectionReason?.Trim();

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException(
                "A rejection reason is required.",
                nameof(request.RejectionReason));
        }

        if (reason.Length > 1000)
        {
            throw new ArgumentException(
                "The rejection reason cannot exceed 1000 characters.",
                nameof(request.RejectionReason));
        }

        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException(
                "An authenticated user is required to reject a batch.");
        }

        var rejectedByUserId = _currentUser.UserId;

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
                "RECEIVED",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Only received green leaf batches can be rejected.");
        }

        var rejectedStatus = await _context.Set<Leafbatchstatus>()
            .SingleOrDefaultAsync(
                x => x.Name == "REJECTED",
                cancellationToken);

        if (rejectedStatus is null)
        {
            throw new InvalidOperationException(
                "The REJECTED status is not configured.");
        }

        var rejectedAt = DateTime.UtcNow;

        batch.LeafBatchStatusId = rejectedStatus.Id;
        batch.LeafBatchStatus = rejectedStatus;
        batch.RejectionReason = reason;
        batch.RejectedAt = rejectedAt;
        batch.RejectedByUserId = rejectedByUserId;
        batch.UpdatedAt = rejectedAt;

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

        return new RejectGreenLeafBatchResponse(
            batch.Id,
            batch.BatchNumber,
            rejectedStatus.Name!,
            reason,
            rejectedAt,
            rejectedByUserId);
    }
}