
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;

public sealed class CompleteGreenLeafProcessingCommandValidator
    : AbstractValidator<CompleteGreenLeafProcessingCommand>
{
    public CompleteGreenLeafProcessingCommandValidator(
        UvaTeaDbContext context)
    {
        RuleFor(x => x.GreenLeafBatchId)
            .GreaterThan(0)
            .WithMessage("Green leaf batch ID must be greater than zero.");

        RuleFor(x => x.ProcessedWeight)
            .GreaterThan(0)
            .WithMessage("Processed weight must be greater than zero.");

        RuleFor(x => x.ProcessingNotes)
            .MaximumLength(1000)
            .WithMessage("Processing notes cannot exceed 1000 characters.");

        RuleFor(x => x.GreenLeafBatchId)
            .MustAsync(async (id, cancellationToken) =>
                await context.Set<Greenleafbatch>()
                    .AnyAsync(
                        batch => batch.Id == id,
                        cancellationToken))
            .WithMessage("Green leaf batch was not found.")
            .When(x => x.GreenLeafBatchId > 0);
    }
}
