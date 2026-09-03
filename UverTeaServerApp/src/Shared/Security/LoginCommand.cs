using MediatR;

namespace UverTeaServerApp.Shared.Security;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;
