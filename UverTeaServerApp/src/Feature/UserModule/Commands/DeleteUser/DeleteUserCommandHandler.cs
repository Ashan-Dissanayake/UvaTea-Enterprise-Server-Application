using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
    private readonly UvaTeaDbContext _context;

    public DeleteUserCommandHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException($"User with ID '{request.Id}' not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
