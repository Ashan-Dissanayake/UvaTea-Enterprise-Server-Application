using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Queries.GetAllEmployees;

public record GetAllEmployeesQuery(PaginationParams? Params = null) 
    : IRequest<PagedResult<EmployeeDetailResponseDto>>, UverTeaServerApp.Shared.Caching.ICacheableQuery
{
    public string CacheKey => $"employees:all:p{Params?.PageNumber ?? 1}:s{Params?.PageSize ?? 10}:q{Params?.SearchTerm ?? string.Empty}:sort{Params?.SortColumn ?? "id"}_{Params?.SortDirection ?? "asc"}";
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
    public bool BypassCache => false;
}