using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.AreaModule.Commands.CreateArea;

public class CreateAreaCommandValidator
    : AbstractValidator<CreateAreaCommand>
{
    private readonly UvaTeaDbContext _context;

    public CreateAreaCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        // ============================================================
        // AREA CODE
        // ============================================================

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Area code is required.")
            .MustAsync(BeUniqueCode)
            .WithMessage(
                "An area with Code '{PropertyValue}' already exists.");

        // ============================================================
        // AREA CATEGORY
        // ============================================================

        RuleFor(x => x.AreacategoryId)
            .MustAsync(AreaCategoryExists)
            .WithMessage(
                "Area Category with ID '{PropertyValue}' not found.");

        // ============================================================
        // PLANTING CONFIGURATION
        // ============================================================

        RuleFor(x => x.PlantingConfigurationId)
            .MustAsync(PlantingConfigurationExists)
            .WithMessage(
                "Planting Configuration with ID '{PropertyValue}' " +
                "not found or is inactive.");

        // ============================================================
        // SUPERVISOR
        // ============================================================

        RuleFor(x => x.SupervisorId)
            .MustAsync(SupervisorExistsAndEligible)
            .When(x => x.SupervisorId.HasValue)
            .WithMessage(
                "The selected supervisor is invalid, inactive, " +
                "or is not assigned as a Supervisor.");

        // ============================================================
        // DATES
        // ============================================================

        RuleFor(x => x)
            .Must(HaveValidDates)
            .WithMessage(
                "Proofing date cannot be later than attached date.");

        // ============================================================
        // PLANT COUNT
        // ============================================================

        RuleFor(x => x)
            .MustAsync(PlantCountIsValid)
            .WithMessage(
                "Plant count exceeds the calculated capacity " +
                 "for the selected area and planting configuration.");
    }

    // ================================================================
    // AREA CODE
    // ================================================================

    private async Task<bool> BeUniqueCode(
        string code,
        CancellationToken cancellationToken)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return !await _context.Areas
            .AnyAsync(
                a => a.Code == normalizedCode,
                cancellationToken);
    }

    // ================================================================
    // AREA CATEGORY
    // ================================================================

    private async Task<bool> AreaCategoryExists(
        int categoryId,
        CancellationToken cancellationToken)
    {
        return await _context.Areacategories
            .AnyAsync(
                c => c.Id == categoryId,
                cancellationToken);
    }

    // ================================================================
    // PLANTING CONFIGURATION
    // ================================================================

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

    // ================================================================
    // SUPERVISOR
    // ================================================================

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

        // Employee must:
        // 1. Exist
        // 2. Have Supervisor designation
        // 3. Be active

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

    // ================================================================
    // DATE VALIDATION
    // ================================================================

    private static bool HaveValidDates(
        CreateAreaCommand request)
    {
        if (!request.Doproofing.HasValue ||
            !request.Doattached.HasValue)
        {
            return true;
        }

        return request.Doproofing.Value <=
               request.Doattached.Value;
    }

    // ================================================================
    // PLANT COUNT VALIDATION
    // ================================================================
    private async Task<bool> PlantCountIsValid(
        CreateAreaCommand request,
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

        return request.Plantcount <= maximumPlantCount;
    }
}