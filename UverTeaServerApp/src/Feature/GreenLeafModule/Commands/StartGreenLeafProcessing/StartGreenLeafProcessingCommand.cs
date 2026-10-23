using MediatR;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .StartGreenLeafProcessing;

public sealed record StartGreenLeafProcessingCommand(
    int GreenLeafBatchId
) : IRequest<StartGreenLeafProcessingResponse>;

public sealed record StartGreenLeafProcessingResponse(
    int GreenLeafBatchId,
    string BatchNumber,
    string Status
);