using MediatR;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetStatusHistory;

public record GetStatusHistoryQuery(
    int AreaId
) : IRequest<List<AreaStatusHistoryResponseDto>>;