using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Caching;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.Shared.Middlewares;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;

    public UpdateEmployeeCommandHandler(
        UvaTeaDbContext context, 
        IUnitOfWork unitOfWork,
        ICacheService cacheService)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
    }

    public async Task<EmployeeDetailResponseDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (employee == null)
        {
            throw new ResourceNotFoundException($"Employee with ID '{request.Id}' not found.");
        }

        request.Adapt(employee);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        // Invalidate employee cache
        await _cacheService.RemoveByPrefixAsync("employees:", cancellationToken);

        return employee.Adapt<EmployeeDetailResponseDto>();
    }
}
