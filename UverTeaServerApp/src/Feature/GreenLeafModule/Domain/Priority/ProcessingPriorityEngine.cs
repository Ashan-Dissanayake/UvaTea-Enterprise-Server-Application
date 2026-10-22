using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;

public class ProcessingPriorityEngine
{
    private static readonly IReadOnlyDictionary<string, int> RiskOrder =
        new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["CRITICAL"] = 1,
            ["HIGH"] = 2,
            ["MEDIUM"] = 3,
            ["LOW"] = 4
        };

    public IReadOnlyList<ProcessingPriorityResult> Rank(
        IEnumerable<Greenleafbatch> eligibleBatches,
        DateTime evaluatedAtUtc)
    {
        ArgumentNullException.ThrowIfNull(eligibleBatches);

        var batches = eligibleBatches
            .Select(batch =>
            {
                var assessment = batch.RiskAssessment
                    ?? throw new InvalidOperationException(
                     "A risk assessment is required for priority ranking.");

                var riskLevel = assessment.RiskLevel?.Trim();

                if (string.IsNullOrWhiteSpace(riskLevel) ||
                    !RiskOrder.TryGetValue(riskLevel, out var riskOrder))
                {
                    throw new InvalidOperationException(
                        $"Batch {batch.BatchNumber} has an invalid risk level.");
                }

                if (batch.ArrivalDateTime > evaluatedAtUtc)
                {
                    throw new InvalidOperationException(
                        $"Batch {batch.BatchNumber} has an arrival time in the future.");
                }

                return new
                {
                    Batch = batch,
                    Assessment = assessment,
                    RiskLevel = riskLevel.ToUpperInvariant(),
                    RiskOrder = riskOrder,
                    WaitingTime = evaluatedAtUtc - batch.ArrivalDateTime
                };
            })
            .OrderBy(x => x.RiskOrder)
            .ThenBy(x => x.Batch.ArrivalDateTime)
            .ThenBy(x => x.Batch.Id)
            .ToList();

        return batches
            .Select((item, index) => new ProcessingPriorityResult(
                BatchId: item.Batch.Id,
                BatchNumber: item.Batch.BatchNumber,
                RiskScore: item.Assessment.RiskScore,
                RiskLevel: item.RiskLevel,
                PriorityRank: index + 1,
                PriorityReason:
                    $"{item.RiskLevel} risk; waiting for " +
                    $"{item.WaitingTime.TotalHours:F1} hours."
            ))
            .ToList();
    }
}