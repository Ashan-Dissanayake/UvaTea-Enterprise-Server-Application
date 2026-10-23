
using MediatR;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed record GetGreenLeafYieldAnalysisQuery(
    DateTime? FromDate,
    DateTime? ToDate,
    int? AreaId
) : IRequest<GreenLeafYieldAnalysisResponse>;
