using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Queries.GetAllEmployees;

public record GetAllEmployeesQuery(PaginationParams? Params = null) : IRequest<PagedResult<EmployeeDetailResponseDto>>;