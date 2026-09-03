using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Queries.GetUsers;

public record GetUsersQuery(PaginationParams? Params = null) 
    : IRequest<PagedResult<UserDetailResponseDto>>, UverTeaServerApp.Shared.Caching.ICacheableQuery
{
    public string CacheKey => $"users:all:p{Params?.PageNumber ?? 1}:s{Params?.PageSize ?? 10}:q{Params?.SearchTerm ?? string.Empty}:sort{Params?.SortColumn ?? "id"}_{Params?.SortDirection ?? "asc"}";
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
    public bool BypassCache => false;
}
