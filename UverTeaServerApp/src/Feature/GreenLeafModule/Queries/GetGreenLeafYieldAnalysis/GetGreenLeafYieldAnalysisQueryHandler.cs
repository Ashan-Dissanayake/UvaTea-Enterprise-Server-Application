using Microsoft.EntityFrameworkCore;
using MediatR;
using UverTeaServerApp.Shared.Data;

// Add the actual UvaTeaDbContext namespace used in your project.

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed class GetGreenLeafYieldAnalysisQueryHandler
    : IRequestHandler<
        GetGreenLeafYieldAnalysisQuery,
        GreenLeafYieldAnalysisResponse>
{
    private readonly UvaTeaDbContext _context;

    public GetGreenLeafYieldAnalysisQueryHandler(UvaTeaDbContext context)
    {
        _context = context;
    }

    public async Task<GreenLeafYieldAnalysisResponse> Handle(
        GetGreenLeafYieldAnalysisQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Greenleafprocessingrecords
            .AsNoTracking()
            .Where(record =>
                record.GreenLeafBatch.LeafBatchStatus.Name == "PROCESSED");

        if (request.FromDate.HasValue)
        {
            var fromDate = request.FromDate.Value.Date;

            query = query.Where(record =>
                record.CompletedAt >= fromDate);
        }

        if (request.ToDate.HasValue)
        {
            // Treat ToDate as an inclusive calendar date.
            var toDateExclusive = request.ToDate.Value.Date.AddDays(1);

            query = query.Where(record =>
                record.CompletedAt < toDateExclusive);
        }

        if (request.AreaId.HasValue)
        {
            var areaId = request.AreaId.Value;

            query = query.Where(record =>
                record.GreenLeafBatch.AreaId == areaId);
        }

        var rows = await query
            .Select(record => new
            {
                GreenLeafBatchId = record.GreenLeafBatch.Id,
                record.GreenLeafBatch.BatchNumber,
                record.GreenLeafBatch.AreaId,
                ReceivedWeight = record.GreenLeafBatch.Weight,
                record.ProcessedWeight,
                record.CompletedAt
            })
            .OrderByDescending(record => record.CompletedAt)
            .ThenBy(record => record.GreenLeafBatchId)
            .ToListAsync(cancellationToken);

        var totalReceivedWeight = rows.Sum(x => x.ReceivedWeight);
        var totalProcessedWeight = rows.Sum(x => x.ProcessedWeight);

        // Calculate the overall yield from aggregate weights,
        // not by averaging individual batch percentages.
        var overallYield = totalReceivedWeight > 0
            ? Math.Round(
                totalProcessedWeight / totalReceivedWeight * 100m,
                2,
                MidpointRounding.AwayFromZero)
            : 0m;

        var batches = rows
            .Select(row => new GreenLeafBatchYieldDto(
                GreenLeafBatchId: row.GreenLeafBatchId,
                BatchNumber: row.BatchNumber,
                AreaId: row.AreaId,
                ReceivedWeight: row.ReceivedWeight,
                ProcessedWeight: row.ProcessedWeight,
                YieldPercentage: row.ReceivedWeight > 0
                    ? Math.Round(
                        row.ProcessedWeight / row.ReceivedWeight * 100m,
                        2,
                        MidpointRounding.AwayFromZero)
                    : 0m,
                CompletedAt: row.CompletedAt
            ))
            .ToList();

        return new GreenLeafYieldAnalysisResponse(
            TotalReceivedWeight: totalReceivedWeight,
            TotalProcessedWeight: totalProcessedWeight,
            OverallYieldPercentage: overallYield,
            CompletedBatchCount: batches.Count,
            Batches: batches
        );
    }
}