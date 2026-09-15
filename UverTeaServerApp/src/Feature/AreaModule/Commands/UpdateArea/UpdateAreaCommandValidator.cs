using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.AreaModule.Commands.UpdateArea;

public class UpdateAreaCommandValidator : AbstractValidator<UpdateAreaCommand>
{
    private readonly UvaTeaDbContext _context;

    public UpdateAreaCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(AreaExistsAndIsActive)
            .WithMessage(
                "Area not found or is already decommissioned.");

        RuleFor(x => x.AreacategoryId)
            .MustAsync(AreaCategoryExists)
            .WithMessage(
                "Area Category with ID '{PropertyValue}' not found.");

        RuleFor(x => x.PlantingConfigurationId)
            .MustAsync(PlantingConfigurationExists)
            .WithMessage(
                "Planting Configuration with ID '{PropertyValue}' " +
                "not found or is inactive.");

        RuleFor(x => x.SupervisorId)
            .MustAsync(SupervisorExistsAndEligible)
            .When(x => x.SupervisorId.HasValue)
            .WithMessage(
                "The selected supervisor is invalid, inactive, " +
                "or is not assigned as a Supervisor.");

        RuleFor(x => x)
            .Must(HaveValidDates)
            .WithMessage(
                "Proofing date cannot be later than attached date.");

        RuleFor(x => x)
            .Must(HaveNoFutureDates)
            .WithMessage(
                "Proofing and attached dates cannot be in the future.");

        RuleFor(x => x)
            .MustAsync(PlantCountIsValid)
            .WithMessage(
                "Plant count exceeds the calculated capacity " +
                "for the selected area and planting configuration.");
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
               area.StatusName != "Decommissioned";
    }

    private async Task<bool> AreaCategoryExists(
        int categoryId,
        CancellationToken cancellationToken)
    {
        return await _context.Areacategories
            .AnyAsync(
                c => c.Id == categoryId,
                cancellationToken);
    }

    private async Task<bool> PlantingConfigurationExists(
        int configurationId,
        CancellationToken cancellationToken)
    {
        return await _context.Plantingconfigurations
            .AnyAsync(
                p =>
                    p.Id == configurationId &&
                    p.Isactive,
                cancellationToken);
    }

    private async Task<bool> SupervisorExistsAndEligible(
        int? supervisorId,
        CancellationToken cancellationToken)
    {
        if (!supervisorId.HasValue)
            return true;

        var supervisorDesignationId =
            await _context.Designations
                .Where(d => d.Name == "Supervisor")
                .Select(d => d.Id)
                .SingleOrDefaultAsync(cancellationToken);

        if (supervisorDesignationId == 0)
            return false;

        var activeEmployeeStatusId =
            await _context.EmployeeStatuses
                .Where(s => s.Name == "Active")
                .Select(s => s.Id)
                .SingleOrDefaultAsync(cancellationToken);

        if (activeEmployeeStatusId == 0)
            return false;

        return await _context.Employees
            .AnyAsync(
                e =>
                    e.Id == supervisorId.Value &&
                    e.DesignationId == supervisorDesignationId &&
                    e.EmployeestatusId == activeEmployeeStatusId,
                cancellationToken);
    }

    private static bool HaveValidDates(
        UpdateAreaCommand request)
    {
        if (!request.Doproofing.HasValue ||
            !request.Doattached.HasValue)
        {
            return true;
        }

        return request.Doproofing.Value <=
               request.Doattached.Value;
    }

    private static bool HaveNoFutureDates(
        UpdateAreaCommand request)
    {
        var today = DateOnly.FromDateTime(
            DateTime.UtcNow);

        if (request.Doproofing.HasValue &&
            request.Doproofing.Value > today)
        {
            return false;
        }

        if (request.Doattached.HasValue &&
            request.Doattached.Value > today)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> PlantCountIsValid(
        UpdateAreaCommand request,
        CancellationToken cancellationToken)
    {
        var configuration =
            await _context.Plantingconfigurations
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p =>
                        p.Id == request.PlantingConfigurationId &&
                        p.Isactive,
                    cancellationToken);

        if (configuration == null)
            return false;

        if (configuration.Rowspacingfeet <= 0 ||
            configuration.Plantspacingfeet <= 0)
        {
            return false;
        }

        if (request.Acres <= 0)
            return false;

        var areaSquareFeet =
            request.Acres * 43_560m;

        var plantCapacity =
            areaSquareFeet /
            (configuration.Rowspacingfeet *
             configuration.Plantspacingfeet);

        var maximumPlantCount =
            Math.Floor(plantCapacity);

        return request.Plantcount <=
               maximumPlantCount;
    }
}