using MediatR;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Events;

public record EmployeeCreatedEvent(
    int EmployeeId,
    string Fullname,
    string? Callingname,
    string? Email
) : INotification;