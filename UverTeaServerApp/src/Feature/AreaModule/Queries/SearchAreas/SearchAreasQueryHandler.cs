using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.SeachAreas;

public class SearchAreasQueryHandler
    : IRequestHandler<SearchAreasQuery, PagedResult<AreaDetailResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public SearchAreasQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<AreaDetailResponseDto>> Handle(
        SearchAreasQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Areas
            .AsNoTracking()
            .Include(e => e.AreaStatus)
            .Include(e => e.AreaCategory)
            .Include(e => e.Supervisor)
            .AsQueryable();

        var paramsDict = request.ParamsDict;

        if (paramsDict != null)
        {
            if (paramsDict.TryGetValue("code", out var code) &&
                !string.IsNullOrEmpty(code))
            {
                query = query.Where(e => e.Code == code);
            }

            if (paramsDict.TryGetValue("areastatusid", out var areastatusid) &&
                int.TryParse(areastatusid, out int sId))
            {
                query = query.Where(e => e.AreaStatusId == sId);
            }

            if (paramsDict.TryGetValue("areacategoryid", out var areacategoryid) &&
                int.TryParse(areacategoryid, out int cId))
            {
                query = query.Where(e => e.AreaCategoryId == cId);
            }

            if (paramsDict.TryGetValue("plantcount", out var plantcount) &&
                int.TryParse(plantcount, out int pCount))
            {
                query = query.Where(e => e.PlantCount == pCount);
            }
        }

        var paginationParams =
            request.Pagination ?? new PaginationParams();

        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.Trim();

            query = query.Where(e =>
                (e.Code != null &&
                 e.Code.Contains(search)) ||

                (e.Supervisor != null &&
                 e.Supervisor.Callingname != null &&
                 e.Supervisor.Callingname.Contains(search)) ||

                (e.Supervisor != null &&
                 e.Supervisor.Fullname != null &&
                 e.Supervisor.Fullname.Contains(search)));
        }

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
            projectedQuery = projectedQuery.OrderByDescending(e => e.Id);
        }

        return await projectedQuery.ToPagedResultAsync(
            paginationParams,
            cancellationToken);
    }
}

