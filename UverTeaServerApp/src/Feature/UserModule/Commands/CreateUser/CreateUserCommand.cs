using MediatR;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.CreateUser;

/// <summary>
/// Command to register a new application user.
/// Password is passed in plain-text here and hashed securely inside the handler.
/// </summary>
public record CreateUserCommand(

    string Username,

    /// <summary>Plain-text password – will be hashed by the handler before persistence.</summary>
    string Password,

    int UserstatusId,

    int EmployeeId,

    int RoleId,

    string? Description

) : IRequest<UserDetailResponseDto>, UverTeaServerApp.Shared.Behaviors.ITransactionalRequest;
