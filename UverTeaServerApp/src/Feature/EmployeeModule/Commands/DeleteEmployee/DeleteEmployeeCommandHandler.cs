using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly UvaTeaDbContext _context;

    public DeleteEmployeeCommandHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        Employee employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new ResourceNotFoundException($"Employee with ID '{request.Id}' not found.");
        }

        
        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}