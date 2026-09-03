using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;

namespace UverTeaServerApp.EmployeeModule.Commands.CreateEmployee;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    private readonly UvaTeaDbContext _context;

    public CreateEmployeeCommandValidator(UvaTeaDbContext context)
    {
        _context = context;

        // Duplicate Checks
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Employee number is required.")
            .MustAsync(BeUniqueNumber).WithMessage("An employee with Number '{PropertyValue}' already exists.");

        RuleFor(x => x.Nic)
            .NotEmpty().WithMessage("NIC is required.")
            .MustAsync(BeUniqueNic).WithMessage("An employee with NIC '{PropertyValue}' already exists.");

        RuleFor(x => x.Mobile)
            .MustAsync((mobile, token) => BeUniqueMobile(mobile!, token))
            .When(x => !string.IsNullOrEmpty(x.Mobile))
            .WithMessage("An employee with Mobile '{PropertyValue}' already exists.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.")
            .MustAsync((email, token) => BeUniqueEmail(email!, token))
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("An employee with Email '{PropertyValue}' already exists.");

        // Foreign Key Cross-Validations (Existence Checks)
        RuleFor(x => x.GenderId)
            .MustAsync(GenderExists).WithMessage("Gender with ID '{PropertyValue}' not found.");

        RuleFor(x => x.DesignationId)
            .MustAsync(DesignationExists).WithMessage("Designation with ID '{PropertyValue}' not found.");

        RuleFor(x => x.EmployeestatusId)
            .MustAsync(StatusExists).WithMessage("Employee Status with ID '{PropertyValue}' not found.");
    }

    private async Task<bool> BeUniqueNumber(string number, CancellationToken cancellationToken) =>
        !await _context.Employees.AnyAsync(e => e.Number == number, cancellationToken);

    private async Task<bool> BeUniqueNic(string nic, CancellationToken cancellationToken) =>
        !await _context.Employees.AnyAsync(e => e.Nic == nic, cancellationToken);

    private async Task<bool> BeUniqueMobile(string mobile, CancellationToken cancellationToken) =>
        !await _context.Employees.AnyAsync(e => e.Mobile == mobile, cancellationToken);

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken) =>
        !await _context.Employees.AnyAsync(e => e.Email == email, cancellationToken);

    private async Task<bool> GenderExists(int genderId, CancellationToken cancellationToken) =>
        await _context.Genders.AnyAsync(g => g.Id == genderId, cancellationToken);

    private async Task<bool> DesignationExists(int designationId, CancellationToken cancellationToken) =>
        await _context.Designations.AnyAsync(d => d.Id == designationId, cancellationToken);

    private async Task<bool> StatusExists(int statusId, CancellationToken cancellationToken) =>
        await _context.EmployeeStatuses.AnyAsync(s => s.Id == statusId, cancellationToken);
}
