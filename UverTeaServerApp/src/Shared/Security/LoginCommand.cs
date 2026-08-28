using MediatR;

namespace UverTeaServerApp.src.Shared.Security;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;