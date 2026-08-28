
using Mapster;
using MediatR;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Events;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;

    public CreateEmployeeCommandHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDetailResponseDto> Handle(
        CreateEmployeeCommand request, CancellationToken cancellationToken)
    {

        var employee = request.Adapt<Employee>();

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return employee.Adapt<EmployeeDetailResponseDto>();
    }

}


