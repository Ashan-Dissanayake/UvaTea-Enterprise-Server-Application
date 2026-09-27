using MediatR;
using UverTeaServerApp.Shared.Caching;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetAllAreas;

public record GetAllAreasQuery(PaginationParams? Params = null) 
    : IRequest<PagedResult<AreaDetailResponseDto>>, ICacheableQuery
{
    public string CacheKey => $"areas:all:p{Params?.PageNumber ?? 1}:s{Params?.PageSize ?? 10}:q{Params?.SearchTerm ?? ""}:sort{Params?.SortColumn ?? "id"}_{Params?.SortDirection ?? "desc"}";
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
    public bool BypassCache => false;
}