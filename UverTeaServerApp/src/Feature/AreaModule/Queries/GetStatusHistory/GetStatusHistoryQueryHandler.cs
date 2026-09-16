using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetStatusHistory;

public class GetStatusHistoryQueryHandler
    : IRequestHandler<
        GetStatusHistoryQuery,
        List<AreaStatusHistoryResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public GetStatusHistoryQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<List<AreaStatusHistoryResponseDto>> Handle(
        GetStatusHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var areaExists = await _context.Areas
            .AsNoTracking()
            .AnyAsync(
                a => a.Id == request.AreaId,
                cancellationToken);

        if (!areaExists)
        {
            throw new ResourceNotFoundException(
                $"Area with ID '{request.AreaId}' not found.");
        }

        var history = await _context.Areastatushistories
            .AsNoTracking()
            .Where(h => h.Area_id == request.AreaId)
            .Include(h => h.From_status)
            .Include(h => h.To_status)
            .OrderByDescending(h => h.Changedat)
            .ToListAsync(cancellationToken);

        return history.Adapt<List<AreaStatusHistoryResponseDto>>();
    }
}