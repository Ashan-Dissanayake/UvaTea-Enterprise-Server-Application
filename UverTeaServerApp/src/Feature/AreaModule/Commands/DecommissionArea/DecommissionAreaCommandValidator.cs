using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DecommissionArea;

public class DecommissionAreaCommandValidator
    : AbstractValidator<DecommissionAreaCommand>
{
    private readonly UvaTeaDbContext _context;

    public DecommissionAreaCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        RuleFor(x => x.AreaId)
            .MustAsync(AreaExistsAndIsActive)
            .WithMessage("Area not found or is already decommissioned.");

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

        return area != null && area.StatusName == "Active";
    }
}