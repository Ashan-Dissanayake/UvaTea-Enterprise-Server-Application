using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;

namespace UverTeaServerApp.src.Feature.UserModule.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;

    public GetUserByIdQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<UserDetailResponseDto> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.Userstatus)
            .Include(u => u.Role)
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new ResourceNotFoundException($"User with ID '{request.Id}' not found.");
        }

        return user.Adapt<UserDetailResponseDto>();
    }
}
