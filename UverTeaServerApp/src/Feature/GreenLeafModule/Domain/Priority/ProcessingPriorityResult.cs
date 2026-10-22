namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;

public record ProcessingPriorityResult(
    int BatchId,
    string BatchNumber,
    int RiskScore,
    string RiskLevel,
    int PriorityRank,
    string PriorityReason
);