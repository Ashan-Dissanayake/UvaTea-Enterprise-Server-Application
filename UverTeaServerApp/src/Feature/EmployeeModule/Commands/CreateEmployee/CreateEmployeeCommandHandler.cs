
using Mapster;
using MediatR;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Events;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublisher _publisher;

    public CreateEmployeeCommandHandler(
        UvaTeaDbContext context, 
        IUnitOfWork unitOfWork,
        IPublisher publisher)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _publisher = publisher;
    }

    public async Task<EmployeeDetailResponseDto> Handle(
        CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = request.Adapt<Employee>();

        _context.Employees.Add(employee);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish domain event to trigger real-time notifications and welcome email
        await _publisher.Publish(new EmployeeCreatedEvent(
            employee.Id,
            employee.Fullname ?? string.Empty,
            employee.Callingname,
            employee.Email
        ), cancellationToken);

        return employee.Adapt<EmployeeDetailResponseDto>();
    }
}



