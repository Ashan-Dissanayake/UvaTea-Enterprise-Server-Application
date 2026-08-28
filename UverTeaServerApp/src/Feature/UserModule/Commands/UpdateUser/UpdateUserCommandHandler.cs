using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;
using Mapster;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly PasswordHasher<User> _passwordHasher;

    public UpdateUserCommandHandler(UvaTeaDbContext context)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<UserDetailResponseDto> Handle(
        UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the tracked entity
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException($"User with ID '{request.Id}' not found.");
        }

        // 2. Apply scalar field updates
        user.Username     = request.Username;
        user.UserstatusId = request.UserstatusId;
        user.RoleId       = request.RoleId;
        user.Description  = request.Description;

        // 3. Conditionally re-hash the password — only when a new one is supplied
        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            user.Password = _passwordHasher.HashPassword(user, request.NewPassword);
        }

        // 4. Updatedat is handled automatically by AuditableEntityInterceptor
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Reload navigations for the response DTO
        await _context.Entry(user).Reference(u => u.Userstatus).LoadAsync(cancellationToken);
        await _context.Entry(user).Reference(u => u.Role).LoadAsync(cancellationToken);

        return user.Adapt<UserDetailResponseDto>();
    }
}
