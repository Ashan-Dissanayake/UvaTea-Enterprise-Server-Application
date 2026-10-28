using FluentValidation;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .RejectGreenLeafBatch;

public sealed class RejectGreenLeafBatchCommandValidator
    : AbstractValidator<RejectGreenLeafBatchCommand>
{
    public RejectGreenLeafBatchCommandValidator()
    {
        RuleFor(x => x.GreenLeafBatchId)
            .GreaterThan(0)
            .WithMessage("Green leaf batch ID must be greater than zero.");

        RuleFor(x => x.RejectionReason)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("A rejection reason is required.")
            .Must(reason => !string.IsNullOrWhiteSpace(reason))
            .WithMessage("A rejection reason cannot contain only whitespace.")
            .Must(reason => reason == reason.Trim())
            .WithMessage("Rejection reason cannot start or end with whitespace.")
            .MaximumLength(1000)
            .WithMessage("Rejection reason cannot exceed 1000 characters.");
    }
}