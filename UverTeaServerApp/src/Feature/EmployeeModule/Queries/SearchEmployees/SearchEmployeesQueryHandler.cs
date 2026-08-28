using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Queries.SeachEmployees;

public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, PagedResult<EmployeeDetailResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public SearchEmployeesQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EmployeeDetailResponseDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Gender)
            .Include(e => e.Designation)
            .Include(e => e.Employeestatus)
            .AsQueryable();

        var paramsDict = request.ParamsDict;

        if (paramsDict != null)
        {
            if (paramsDict.TryGetValue("number", out var number) && !string.IsNullOrEmpty(number))
                query = query.Where(e => e.Number == number);

            if (paramsDict.TryGetValue("designationid", out var designationid) && int.TryParse(designationid, out int dId))
                query = query.Where(e => e.DesignationId == dId);

            if (paramsDict.TryGetValue("nic", out var nic) && !string.IsNullOrEmpty(nic))
                query = query.Where(e => e.Nic != null && e.Nic.Contains(nic));
        }

        var paginationParams = request.Pagination ?? new PaginationParams();

        if (!string.IsNullOrWhiteSpace(paginationParams.SearchTerm))
        {
            var search = paginationParams.SearchTerm.Trim();
            query = query.Where(e =>
                (e.Fullname != null && e.Fullname.Contains(search)) ||
                (e.Callingname != null && e.Callingname.Contains(search)) ||
                (e.Number != null && e.Number.Contains(search)) ||
                (e.Nic != null && e.Nic.Contains(search)) ||
                (e.Mobile != null && e.Mobile.Contains(search)));
        }

        var projectedQuery = query.ProjectToType<EmployeeDetailResponseDto>();

        if (!string.IsNullOrWhiteSpace(paginationParams.SortColumn))
        {
            projectedQuery = projectedQuery.ApplySort(paginationParams.SortColumn, paginationParams.SortDirection);
        }
        else
        {
            projectedQuery = projectedQuery.OrderByDescending(e => e.Id);
        }

        return await projectedQuery.ToPagedResultAsync(paginationParams, cancellationToken);
    }
}