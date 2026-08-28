using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.Shared.Extensions;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Queries.GetAllEmployees;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PagedResult<EmployeeDetailResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public GetAllEmployeesQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EmployeeDetailResponseDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var paginationParams = request.Params ?? new PaginationParams();

        var query = _context.Employees
            .AsNoTracking()
            .Include(e => e.Gender)
            .Include(e => e.Designation)
            .Include(e => e.Employeestatus)
            .AsQueryable();

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