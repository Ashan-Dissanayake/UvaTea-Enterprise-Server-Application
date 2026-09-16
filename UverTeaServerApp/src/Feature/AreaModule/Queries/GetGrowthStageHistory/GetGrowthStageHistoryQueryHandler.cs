using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Queries.GetGrowthStageHistory;

public class GetGrowthStageHistoryQueryHandler
    : IRequestHandler<
        GetGrowthStageHistoryQuery,
        List<AreaGrowthStageHistoryResponseDto>>
{
    private readonly UvaTeaDbContext _context;

    public GetGrowthStageHistoryQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<List<AreaGrowthStageHistoryResponseDto>> Handle(
        GetGrowthStageHistoryQuery request,
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

        var history = await _context.Areagrowthstagehistories
            .AsNoTracking()
            .Where(h => h.Area_id == request.AreaId)
            .Include(h => h.From_growthstage)
            .Include(h => h.To_growthstage)
            .OrderByDescending(h => h.Changedat)
            .ToListAsync(cancellationToken);

        return history.Adapt<List<AreaGrowthStageHistoryResponseDto>>();
    }
}