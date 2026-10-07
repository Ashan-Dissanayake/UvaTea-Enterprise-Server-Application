using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class LeafConditionRule : IRiskRule
{
    public RiskRuleResult Evaluate(Greenleafbatch batch)
    {
        var condition =
            batch.LeafCondition.Name?.Trim().ToUpperInvariant();

        return condition switch
        {
            "FRESH" => new RiskRuleResult(
                "Leaf Condition",
                0,
                "Leaf condition is fresh and does not introduce additional risk."),

            "NORMAL" => new RiskRuleResult(
                "Leaf Condition",
                5,
                "Leaf condition is normal with a minor risk contribution."),

            "WILTED" => new RiskRuleResult(
                "Leaf Condition",
                20,
                "Leaf condition indicates wilting and contributes significant risk."),

            "DAMAGED" => new RiskRuleResult(
                "Leaf Condition",
                30,
                "Leaf condition indicates damaged leaves and contributes critical risk."),

            _ => new RiskRuleResult(
                "Leaf Condition",
                0,
                "Leaf condition is unknown and no additional risk score was assigned.")
        };
    }
}