using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Data;

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
            .MustAsync(BeUniqueMobile).When(x => !string.IsNullOrEmpty(x.Mobile))
            .WithMessage("An employee with Mobile '{PropertyValue}' already exists.");

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

    private async Task<bool> GenderExists(int genderId, CancellationToken cancellationToken) =>
        await _context.Genders.AnyAsync(g => g.Id == genderId, cancellationToken);

    private async Task<bool> DesignationExists(int designationId, CancellationToken cancellationToken) =>
        await _context.Designations.AnyAsync(d => d.Id == designationId, cancellationToken);

    private async Task<bool> StatusExists(int statusId, CancellationToken cancellationToken) =>
        await _context.EmployeeStatuses.AnyAsync(s => s.Id == statusId, cancellationToken);
}