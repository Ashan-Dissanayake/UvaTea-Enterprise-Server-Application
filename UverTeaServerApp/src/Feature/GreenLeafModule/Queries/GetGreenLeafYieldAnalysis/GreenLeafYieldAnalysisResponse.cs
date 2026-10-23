
namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed record GreenLeafYieldAnalysisResponse(
    decimal TotalReceivedWeight,
    decimal TotalProcessedWeight,
    decimal OverallYieldPercentage,
    int CompletedBatchCount,
    IReadOnlyCollection<GreenLeafBatchYieldDto> Batches
);

public sealed record GreenLeafBatchYieldDto(
    int GreenLeafBatchId,
    string BatchNumber,
    int AreaId,
    decimal ReceivedWeight,
    decimal ProcessedWeight,
    decimal YieldPercentage,
    DateTime CompletedAt
);
