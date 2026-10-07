using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;

public class TransportDelayRule : IRiskRule
{
    public int Evaluate(Greenleafbatch batch)
    {
        var transportTime =
            batch.ArrivalDateTime - batch.CollectionDateTime;

        if (transportTime <= TimeSpan.FromHours(1))
            return 0;

        if (transportTime <= TimeSpan.FromHours(2))
            return 10;

        if (transportTime <= TimeSpan.FromHours(3))
            return 20;

        return 30;
    }
}