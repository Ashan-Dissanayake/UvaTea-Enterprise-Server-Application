using FluentValidation;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed class GetGreenLeafYieldAnalysisQueryValidator
    : AbstractValidator<GetGreenLeafYieldAnalysisQuery>
{
    public GetGreenLeafYieldAnalysisQueryValidator()
    {
        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
            .WithMessage("FromDate must be earlier than or equal to ToDate.");

        RuleFor(x => x.AreaId)
            .GreaterThan(0)
            .When(x => x.AreaId.HasValue)
            .WithMessage("AreaId must be greater than zero.");
    }
}