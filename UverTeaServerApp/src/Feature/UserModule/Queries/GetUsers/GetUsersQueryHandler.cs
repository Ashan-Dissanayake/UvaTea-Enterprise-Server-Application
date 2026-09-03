using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDetailResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public GetUsersQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<UserDetailResponseDto>> Handle(
        GetUsersQuery request, CancellationToken cancellationToken)
    {
        var paginationParams = request.Params ?? new PaginationParams();

        var query = _context.Users
            .AsNoTracking()
            .Include(u => u.Userstatus)
            .Include(u => u.Role)
            .Include(u => u.Employee)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.Trim();
            query = query.Where(u =>
                (u.Username != null && u.Username.Contains(search)) ||
                (u.Role != null && u.Role.Name != null && u.Role.Name.Contains(search)));
        }

        var projectedQuery = query.ProjectToType<UserDetailResponseDto>();

        if (!string.IsNullOrWhiteSpace(paginationParams.SortColumn))
        {
            projectedQuery = projectedQuery.ApplySort(paginationParams.SortColumn, paginationParams.SortDirection);
        }
        else
        {
            projectedQuery = projectedQuery.OrderByDescending(u => u.Id);
        }

        return await projectedQuery.ToPagedResultAsync(paginationParams, cancellationToken);
    }
}

