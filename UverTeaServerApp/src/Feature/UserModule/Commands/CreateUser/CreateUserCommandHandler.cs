using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PasswordHasher<User> _passwordHasher;

    public CreateUserCommandHandler(UvaTeaDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDetailResponseDto> Handle(
        CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Map scalar fields from the command to the entity
        var user = request.Adapt<User>();

        // Hash the plain-text password before persisting
        // A temporary User instance is used as the hasher context (required by the API).
        user.Password = _passwordHasher.HashPassword(user, request.Password);

        // Auditable fields (Createdat / Updatedat) are handled automatically
        // by the AuditableEntityInterceptor registered in Program.cs.

        _context.Users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload the entity with its navigation properties populated so that
        // the response DTO (Userstatus, Role) is fully hydrated.
        await _context.Entry(user).Reference(u => u.Userstatus).LoadAsync(cancellationToken);
        await _context.Entry(user).Reference(u => u.Role).LoadAsync(cancellationToken);

        return user.Adapt<UserDetailResponseDto>();
    }
}

