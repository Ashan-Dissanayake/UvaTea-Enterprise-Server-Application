using MediatR;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .QueueGreenLeafBatch;

public sealed record QueueGreenLeafBatchCommand(
    int GreenLeafBatchId)
    : IRequest<QueueGreenLeafBatchResponse>;

public sealed record QueueGreenLeafBatchResponse(
    int GreenLeafBatchId,
    string BatchNumber,
    string Status);