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
        var riskScore = _rules.Sum(
            rule => rule.Evaluate(batch));

        var riskLevel = DetermineRiskLevel(riskScore);

        return Task.FromResult(
            new RiskAssessmentResult(
                riskScore,
                riskLevel));
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