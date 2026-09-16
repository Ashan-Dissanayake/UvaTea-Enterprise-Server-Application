using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.AreaModule.Commands.ChangeGrowthStage;

public class ChangeGrowthStageCommandValidator
    : AbstractValidator<ChangeGrowthStageCommand>
{
    private readonly UvaTeaDbContext _context;

    public ChangeGrowthStageCommandValidator(
        UvaTeaDbContext context)
    {
        _context = context;

        RuleFor(x => x.AreaId)
            .MustAsync(AreaExistsAndIsActive)
            .WithMessage(
                "Area not found or is already decommissioned.");

        RuleFor(x => x.GrowthStageId)
            .MustAsync(GrowthStageExistsAndIsActive)
            .WithMessage(
                "Growth Stage with ID '{PropertyValue}' " +
                "not found or is inactive.");

        RuleFor(x => x)
            .MustAsync(IsValidGrowthStageTransition)
            .WithMessage(
                "The requested Growth Stage transition is not allowed.");

        RuleFor(x => x.RowVersion)
                .NotEmpty()
            .WithMessage("Row version is required.");
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

    private async Task<bool> GrowthStageExistsAndIsActive(
        int growthStageId,
        CancellationToken cancellationToken)
    {
        return await _context.Growthstages
            .AnyAsync(
                g =>
                    g.Id == growthStageId &&
                    g.Isactive,
                cancellationToken);
    }

    private async Task<bool> IsValidGrowthStageTransition(
        ChangeGrowthStageCommand request,
        CancellationToken cancellationToken)
    {
        var currentStage = await _context.Areas
            .AsNoTracking()
            .Where(a => a.Id == request.AreaId)
            .Select(a => a.GrowthStage)
            .FirstOrDefaultAsync(cancellationToken);

        if (currentStage == null)
            return false;

        var requestedStage = await _context.Growthstages
            .AsNoTracking()
            .FirstOrDefaultAsync(
                g =>
                    g.Id == request.GrowthStageId &&
                    g.Isactive,
                cancellationToken);

        if (requestedStage == null)
            return false;

        return IsAllowedTransition(
            currentStage.Code,
            requestedStage.Code);
    }

    private static bool IsAllowedTransition(
        string currentStage,
        string requestedStage)
    {
        return currentStage switch
        {
            "NEWLY_PLANTED" =>
                requestedStage == "BUDDING",

            "BUDDING" =>
                requestedStage == "YOUNG_SHOOT",

            "YOUNG_SHOOT" =>
                requestedStage == "PLUCKING",

            "PLUCKING" =>
                requestedStage == "COARSE_SHOOT",

            "COARSE_SHOOT" =>
                requestedStage == "REGROWTH",

            "REGROWTH" =>
                requestedStage == "BUDDING",

            _ => false
        };
    }
}