using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DeactivatePlantingConfiguration;

public class DeactivatePlantingConfigurationCommandValidator
    : AbstractValidator<DeactivatePlantingConfigurationCommand>
{
    private readonly UvaTeaDbContext _context;

    public DeactivatePlantingConfigurationCommandValidator(
        UvaTeaDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(PlantingConfigurationExistsAndIsActive)
            .WithMessage(
                "Planting Configuration not found or is already inactive.");
    }

    private async Task<bool> PlantingConfigurationExistsAndIsActive(
        int id,
        CancellationToken cancellationToken)
    {
        return await _context.Plantingconfigurations
            .AsNoTracking()
            .AnyAsync(
                p => p.Id == id && p.Isactive,
                cancellationToken);
    }
}