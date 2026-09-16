using MediatR;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetGrowthStageHistory;

public record GetGrowthStageHistoryQuery(
    int AreaId
) : IRequest<List<AreaGrowthStageHistoryResponseDto>>;