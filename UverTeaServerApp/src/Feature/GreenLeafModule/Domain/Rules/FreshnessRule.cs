using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class FreshnessRule : IRiskRule
{
    public RiskRuleResult Evaluate(Greenleafbatch batch)
    {
        var elapsedTime =
            batch.ArrivalDateTime - batch.PluckingDateTime;

        if (elapsedTime <= TimeSpan.FromHours(2))
        {
            return new RiskRuleResult(
                "Freshness",
                0,
                "Green leaf arrived within the acceptable freshness period.");
        }

        if (elapsedTime <= TimeSpan.FromHours(4))
        {
            return new RiskRuleResult(
                "Freshness",
                10,
                "Green leaf has a moderate freshness delay.");
        }

        if (elapsedTime <= TimeSpan.FromHours(6))
        {
            return new RiskRuleResult(
                "Freshness",
                20,
                "Green leaf has a significant freshness delay.");
        }

        return new RiskRuleResult(
            "Freshness",
            30,
            "Green leaf has exceeded the critical freshness period.");
    }
}