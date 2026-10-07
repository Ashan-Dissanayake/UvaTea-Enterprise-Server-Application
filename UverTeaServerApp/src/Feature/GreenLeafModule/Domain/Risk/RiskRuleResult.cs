namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;

public record RiskRuleResult(
    string RuleName,
    int Score,
    string? Reason
);