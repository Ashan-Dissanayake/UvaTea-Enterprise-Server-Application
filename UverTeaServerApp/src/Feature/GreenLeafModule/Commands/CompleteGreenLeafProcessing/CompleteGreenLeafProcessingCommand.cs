
using MediatR;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;

public sealed record CompleteGreenLeafProcessingCommand(
    int GreenLeafBatchId,
    decimal ProcessedWeight,
    string? ProcessingNotes
) : IRequest<CompleteGreenLeafProcessingResponse>;

public sealed record CompleteGreenLeafProcessingResponse(
    int GreenLeafBatchId,
    string BatchNumber,
    decimal ProcessedWeight,
    string? ProcessingNotes,
    DateTime CompletedAt,
    string Status
);
