using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetAllAreas;

public class GetAllAreasQueryHandler
    : IRequestHandler<GetAllAreasQuery, PagedResult<AreaDetailResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public GetAllAreasQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AreaDetailResponseDto>> Handle(
        GetAllAreasQuery request,
        CancellationToken cancellationToken)
    {
        var paginationParams =
            request.Params ?? new PaginationParams();

        var query = _context.Areas
            .AsNoTracking()
            .Include(e => e.AreaStatus)
            .Include(e => e.AreaCategory)
            .Include(e => e.Supervisor)
            .AsQueryable();

        var projectedQuery =
            query.ProjectToType<AreaDetailResponseDto>();

        if (!string.IsNullOrWhiteSpace(paginationParams.SortColumn))
        {
            projectedQuery = projectedQuery.ApplySort(
                paginationParams.SortColumn,
                paginationParams.SortDirection);
        }
        else
        {
            projectedQuery =
                projectedQuery.OrderByDescending(e => e.Id);
        }

        return await projectedQuery.ToPagedResultAsync(
            paginationParams,
            cancellationToken);
    }
}