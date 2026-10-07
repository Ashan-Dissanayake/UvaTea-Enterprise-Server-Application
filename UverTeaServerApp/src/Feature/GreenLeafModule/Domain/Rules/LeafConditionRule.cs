using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class LeafConditionRule : IRiskRule
{
    public int Evaluate(Greenleafbatch batch)
    {
        var condition =
            batch.LeafCondition.Name?.Trim().ToUpperInvariant();

        return condition switch
        {
            "FRESH" => 0,
            "NORMAL" => 5,
            "WILTED" => 20,
            "DAMAGED" => 30,

            _ => 0
        };
    }
}