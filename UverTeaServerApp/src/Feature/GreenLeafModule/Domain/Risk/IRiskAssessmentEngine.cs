using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;

public interface IRiskAssessmentEngine
{
    Task<RiskAssessmentResult> AssessAsync(
        Greenleafbatch batch,
        CancellationToken cancellationToken);
}

