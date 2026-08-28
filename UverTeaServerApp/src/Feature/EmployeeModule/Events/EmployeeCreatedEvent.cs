using MediatR;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Events;

public record EmployeeCreatedEvent(
    int EmployeeId,
    string FirstName,
    string LastName,
    string Email
) : INotification;