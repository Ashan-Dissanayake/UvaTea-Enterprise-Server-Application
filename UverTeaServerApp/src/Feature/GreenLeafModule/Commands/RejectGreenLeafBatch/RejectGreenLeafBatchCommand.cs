using MediatR;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .RejectGreenLeafBatch;

public sealed record RejectGreenLeafBatchCommand(
    int GreenLeafBatchId,
    string RejectionReason)
    : IRequest<RejectGreenLeafBatchResponse>;

public sealed record RejectGreenLeafBatchResponse(
    int GreenLeafBatchId,
    string BatchNumber,
    string Status,
    string RejectionReason,
    DateTime RejectedAt,
    int RejectedByUserId);