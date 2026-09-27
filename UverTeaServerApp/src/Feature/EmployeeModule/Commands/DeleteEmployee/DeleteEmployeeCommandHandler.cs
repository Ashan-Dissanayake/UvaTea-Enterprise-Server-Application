using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Caching;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.Shared.Middlewares;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public DeleteEmployeeCommandHandler(
        UvaTeaDbContext context, 
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new ResourceNotFoundException($"Employee with ID '{request.Id}' not found.");
        }
        
        _context.Employees.Remove(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Invalidate employee cache
        await _cacheService.RemoveByPrefixAsync("employees:", cancellationToken);

        return Unit.Value;
    }
}
