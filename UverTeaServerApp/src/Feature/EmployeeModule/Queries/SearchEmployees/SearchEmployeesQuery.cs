using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Queries.SeachEmployees;

public record SearchEmployeesQuery(
    Dictionary<string, string?> ParamsDict,
    PaginationParams? Pagination = null) : IRequest<PagedResult<EmployeeDetailResponseDto>>;