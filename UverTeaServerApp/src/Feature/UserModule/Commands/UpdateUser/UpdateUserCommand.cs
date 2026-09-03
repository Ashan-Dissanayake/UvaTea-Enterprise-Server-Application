using MediatR;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.UpdateUser;

public record UpdateUserCommand(

    int Id,

    string Username,

    /// <summary>
    /// Optional. If null or empty, the existing password hash is preserved unchanged.
    /// If provided, it will be re-hashed before persisting.
    /// </summary>
    string? NewPassword,

    int UserstatusId,

    int RoleId,

    string? Description

) : IRequest<UserDetailResponseDto>, UverTeaServerApp.Shared.Behaviors.ITransactionalRequest;
