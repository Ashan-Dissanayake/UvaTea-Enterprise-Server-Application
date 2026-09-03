using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.src.Feature.UserModule.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly UvaTeaDbContext _context;

    public CreateUserCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        // ── Required fields ────────────────────────────────────────────────

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(45).WithMessage("Username cannot exceed 45 characters.")
            .MustAsync(BeUniqueUsername)
            .WithMessage("A user with username '{PropertyValue}' already exists.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.");

        // ── Foreign Key existence checks ──────────────────────────────────

        RuleFor(x => x.UserstatusId)
            .GreaterThan(0).WithMessage("A valid User Status ID is required.")
            .MustAsync(UserStatusExists)
            .WithMessage("User Status with ID '{PropertyValue}' not found.");

        RuleFor(x => x.EmployeeId)
            .GreaterThan(0).WithMessage("A valid Employee ID is required.")
            .MustAsync(EmployeeExists)
            .WithMessage("Employee with ID '{PropertyValue}' not found.")
            .MustAsync(EmployeeNotAlreadyLinked)
            .WithMessage("Employee with ID '{PropertyValue}' already has a user account.");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("A valid Role ID is required.")
            .MustAsync(RoleExists)
            .WithMessage("Role with ID '{PropertyValue}' not found.");
    }

    // ── Private async predicate methods ──────────────────────────────────────

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken) =>
        !await _context.Users.AnyAsync(u => u.Username == username, cancellationToken);

    private async Task<bool> UserStatusExists(int statusId, CancellationToken cancellationToken) =>
        await _context.Userstatuses.AnyAsync(s => s.Id == statusId, cancellationToken);

    private async Task<bool> EmployeeExists(int employeeId, CancellationToken cancellationToken) =>
        await _context.Employees.AnyAsync(e => e.Id == employeeId, cancellationToken);

    private async Task<bool> EmployeeNotAlreadyLinked(int employeeId, CancellationToken cancellationToken) =>
        !await _context.Users.AnyAsync(u => u.EmployeeId == employeeId, cancellationToken);

    private async Task<bool> RoleExists(int roleId, CancellationToken cancellationToken) =>
        await _context.Roles.AnyAsync(r => r.Id == roleId, cancellationToken);
}

