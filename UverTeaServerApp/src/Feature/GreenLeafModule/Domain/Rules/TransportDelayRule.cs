using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class TransportDelayRule : IRiskRule
{
    public RiskRuleResult Evaluate(Greenleafbatch batch)
    {
        var transportTime =
            batch.ArrivalDateTime - batch.CollectionDateTime;

        if (transportTime <= TimeSpan.FromHours(1))
        {
            return new RiskRuleResult(
                "Transport Delay",
                0,
                "Green leaf arrived within the acceptable transport time.");
        }

        if (transportTime <= TimeSpan.FromHours(2))
        {
            return new RiskRuleResult(
                "Transport Delay",
                10,
                "Green leaf experienced a moderate transport delay.");
        }

        if (transportTime <= TimeSpan.FromHours(3))
        {
            return new RiskRuleResult(
                "Transport Delay",
                20,
                "Green leaf experienced a significant transport delay.");
        }

        return new RiskRuleResult(
            "Transport Delay",
            30,
            "Green leaf experienced a critical transport delay.");
    }
}