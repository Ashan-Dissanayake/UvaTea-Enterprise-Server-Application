using MediatR;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.DeleteUser;

public record DeleteUserCommand(int Id) : IRequest<Unit>, UverTeaServerApp.Shared.Behaviors.ITransactionalRequest;
