namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;

public record RiskAssessmentResult(
    int RiskScore,
    string RiskLevel,
    IReadOnlyCollection<RiskRuleResult> RuleResults
);