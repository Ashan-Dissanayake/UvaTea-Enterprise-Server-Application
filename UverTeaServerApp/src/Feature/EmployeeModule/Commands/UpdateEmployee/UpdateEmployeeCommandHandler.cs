using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;

    public UpdateEmployeeCommandHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDetailResponseDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {

        Employee employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new ResourceNotFoundException($"Employee with ID '{request.Id}' not found.");
        }

        request.Adapt(employee);

        await _context.SaveChangesAsync(cancellationToken);

        return employee.Adapt<EmployeeDetailResponseDto>();
    }
}