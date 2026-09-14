using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.SeachAreas;

public record SearchAreasQuery(
    Dictionary<string, string?> ParamsDict,
    PaginationParams? Pagination = null) : IRequest<PagedResult<AreaDetailResponseDto>>;