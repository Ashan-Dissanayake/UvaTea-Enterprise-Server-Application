using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.GreenLeafModule.Commands.ReceiveGreenLeafBatch;

public class ReceiveGreenLeafBatchCommandValidator
    : AbstractValidator<ReceiveGreenLeafBatchCommand>
{
    private readonly UvaTeaDbContext _context;

    public ReceiveGreenLeafBatchCommandValidator(
        UvaTeaDbContext context)
    {
        _context = context;

        RuleFor(x => x.AreaId)
            .MustAsync(AreaExistsAndIsActive)
            .WithMessage(
                "Area not found or is already decommissioned.");

        RuleFor(x => x.ContainerTypeId)
            .MustAsync(ContainerTypeExists)
            .WithMessage(
                "Container Type with ID '{PropertyValue}' not found.");

        RuleFor(x => x.WeatherConditionId)
            .MustAsync(WeatherConditionExists)
            .WithMessage(
                "Weather Condition with ID '{PropertyValue}' not found.");

        RuleFor(x => x.LeafConditionId)
            .MustAsync(LeafConditionExists)
            .WithMessage(
                "Leaf Condition with ID '{PropertyValue}' not found.");

        RuleFor(x => x)
            .Must(HaveValidTimeline)
            .WithMessage(
                "Plucking time must be earlier than or equal to " +
                "Collection time, and Collection time must be earlier " +
                "than or equal to Arrival time.");
    }

    private async Task<bool> AreaExistsAndIsActive(
        int areaId,
        CancellationToken cancellationToken)
    {
        var area = await _context.Areas
            .AsNoTracking()
            .Where(a => a.Id == areaId)
            .Select(a => new
            {
                a.Id,
                StatusName = a.AreaStatus.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        return area != null &&
               area.StatusName == "Active";
    }

    private async Task<bool> ContainerTypeExists(
        int containerTypeId,
        CancellationToken cancellationToken)
    {
        return await _context.Containertypes
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == containerTypeId,
                cancellationToken);
    }

    private async Task<bool> WeatherConditionExists(
        int weatherConditionId,
        CancellationToken cancellationToken)
    {
        return await _context.Weatherconditions
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == weatherConditionId,
                cancellationToken);
    }

    private async Task<bool> LeafConditionExists(
        int leafConditionId,
        CancellationToken cancellationToken)
    {
        return await _context.Leafconditions
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == leafConditionId,
                cancellationToken);
    }

    private static bool HaveValidTimeline(
        ReceiveGreenLeafBatchCommand request)
    {
        return request.PluckingDateTime
               <= request.CollectionDateTime
               &&
               request.CollectionDateTime
               <= request.ArrivalDateTime;
    }
}