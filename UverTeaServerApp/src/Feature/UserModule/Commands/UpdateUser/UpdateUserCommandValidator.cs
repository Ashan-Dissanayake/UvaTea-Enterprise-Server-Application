using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private readonly UvaTeaDbContext _context;

    public UpdateUserCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        // ── Required fields ───────────────────────────────────────────────

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("A valid User ID is required.");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(45).WithMessage("Username cannot exceed 45 characters.")
            .MustAsync(BeUniqueUsername)
            .WithMessage("A user with username '{PropertyValue}' already exists.");

        // ── Password is optional on update, but must meet rules if supplied ─

        RuleFor(x => x.NewPassword)
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.NewPassword));

        // ── Foreign key existence checks ──────────────────────────────────

        RuleFor(x => x.UserstatusId)
            .GreaterThan(0).WithMessage("A valid User Status ID is required.")
            .MustAsync(UserStatusExists)
            .WithMessage("User Status with ID '{PropertyValue}' not found.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("A valid Role ID is required.")
            .MustAsync(RoleExists)
            .WithMessage("Role with ID '{PropertyValue}' not found.");
    }

    // ── Private async predicate methods ──────────────────────────────────────

    // Self-exclusion: a username is unique as long as no *other* user already has it
    private async Task<bool> BeUniqueUsername(UpdateUserCommand command, string username, CancellationToken ct) =>
        !await _context.Users.AnyAsync(u => u.Id != command.Id && u.Username == username, ct);

    private async Task<bool> UserStatusExists(int statusId, CancellationToken ct) =>
        await _context.Userstatuses.AnyAsync(s => s.Id == statusId, ct);

    private async Task<bool> RoleExists(int roleId, CancellationToken ct) =>
        await _context.Roles.AnyAsync(r => r.Id == roleId, ct);
}
