using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Dtos;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.GreenLeafModule.Commands.ReceiveGreenLeafBatch;

public class ReceiveGreenLeafBatchCommandHandler
    : IRequestHandler<
        ReceiveGreenLeafBatchCommand,
        GreenLeafIntakeResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRiskAssessmentEngine _riskAssessmentEngine;

    public ReceiveGreenLeafBatchCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork,
        IRiskAssessmentEngine riskAssessmentEngine)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _riskAssessmentEngine = riskAssessmentEngine;
    }

    public async Task<GreenLeafIntakeResponseDto> Handle(
        ReceiveGreenLeafBatchCommand request,
        CancellationToken cancellationToken)
    {
        var receivedStatus = await _context.Leafbatchstatuses
            .SingleOrDefaultAsync(
                x => x.Name == "RECEIVED",
                cancellationToken);

        if (receivedStatus == null)
        {
            throw new ResourceNotFoundException(
                "Leaf Batch Status 'RECEIVED' not found.");
        }

        var batch = new Greenleafbatch
        {
            BatchNumber = GenerateBatchNumber(),

            AreaId = request.AreaId,
            ContainerTypeId = request.ContainerTypeId,
            WeatherConditionId = request.WeatherConditionId,
            LeafConditionId = request.LeafConditionId,
            LeafBatchStatusId = receivedStatus.Id,

            PluckingDateTime = request.PluckingDateTime,
            CollectionDateTime = request.CollectionDateTime,
            ArrivalDateTime = request.ArrivalDateTime,

            Weight = request.Weight,

            Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                ? null
                : request.Remarks.Trim(),

            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Greenleafbatches.Add(batch);

        var riskAssessment =
            await _riskAssessmentEngine.AssessAsync(
                batch,
                cancellationToken);

        var riskAssessmentEntity = new Greenleafriskassessment
        {
            GreenLeafBatch = batch,
            RiskScore = riskAssessment.RiskScore,
            RiskLevel = riskAssessment.RiskLevel,
            RiskReason = string.Join(
                "; ",
                riskAssessment.RuleResults
                    .Where(x =>
                        !string.IsNullOrWhiteSpace(x.Reason))
                    .Select(x => x.Reason)),
            AssessedAt = DateTime.UtcNow
        };

        _context.Greenleafriskassessment.Add(
            riskAssessmentEntity);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var createdBatch = await _context.Greenleafbatches
            .AsNoTracking()
            .Include(x => x.Area)
            .Include(x => x.ContainerType)
            .Include(x => x.WeatherCondition)
            .Include(x => x.LeafCondition)
            .Include(x => x.LeafBatchStatus)
            .Include(x => x.GreenLeafBatch)
            .SingleAsync(
                x => x.Id == batch.Id,
                cancellationToken);

        return createdBatch.Adapt<GreenLeafIntakeResponseDto>();
    }

    private static string GenerateBatchNumber()
    {
        return $"GL-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
    }
}