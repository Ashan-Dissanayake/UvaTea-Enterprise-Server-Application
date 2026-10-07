using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;

public class RiskAssessmentEngine : IRiskAssessmentEngine
{
    private readonly IEnumerable<IRiskRule> _rules;

    public RiskAssessmentEngine(
        IEnumerable<IRiskRule> rules)
    {
        _rules = rules;
    }

    public Task<RiskAssessmentResult> AssessAsync(
        Greenleafbatch batch,
        CancellationToken cancellationToken)
    {
        var ruleResults = _rules
            .Select(rule => rule.Evaluate(batch))
            .ToList();

        var riskScore = ruleResults.Sum(
            result => result.Score);

        var riskLevel = DetermineRiskLevel(riskScore);

        return Task.FromResult(
            new RiskAssessmentResult(
                riskScore,
                riskLevel,
                ruleResults));
    }

    private static string DetermineRiskLevel(
        int riskScore)
    {
        return riskScore switch
        {
            <= 20 => "LOW",
            <= 40 => "MEDIUM",
            <= 60 => "HIGH",
            _ => "CRITICAL"
        };
    }
}