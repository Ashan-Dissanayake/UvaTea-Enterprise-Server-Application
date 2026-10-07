using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;

public interface IRiskRule
{
    RiskRuleResult Evaluate(Greenleafbatch batch);
}