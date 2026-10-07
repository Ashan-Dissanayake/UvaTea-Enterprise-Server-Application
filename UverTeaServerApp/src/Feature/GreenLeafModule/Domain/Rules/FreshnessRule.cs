using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class FreshnessRule : IRiskRule
{
    public int Evaluate(Greenleafbatch batch)
    {
        var elapsedTime =
            batch.ArrivalDateTime - batch.PluckingDateTime;

        if (elapsedTime <= TimeSpan.FromHours(2))
            return 0;

        if (elapsedTime <= TimeSpan.FromHours(4))
            return 10;

        if (elapsedTime <= TimeSpan.FromHours(6))
            return 20;

        return 30;
    }
}