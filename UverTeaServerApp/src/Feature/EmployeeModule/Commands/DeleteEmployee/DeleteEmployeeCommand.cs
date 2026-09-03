using System.ComponentModel.DataAnnotations;
using MediatR;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Commands.DeleteEmployee;

public record DeleteEmployeeCommand(
    [Required]
    int Id
) : IRequest<Unit>, UverTeaServerApp.Shared.Behaviors.ITransactionalRequest;