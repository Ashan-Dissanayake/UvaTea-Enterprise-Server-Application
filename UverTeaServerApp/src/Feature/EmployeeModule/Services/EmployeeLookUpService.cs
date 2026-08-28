
using Microsoft.EntityFrameworkCore;
using Mapster;
using UverTeaServerApp.Data;
using UverTeaServerApp.src.shared.Middlewares.Exceptions;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Services;


public class EmployeeLookupService
{
    private readonly UvaTeaDbContext _context;

    public EmployeeLookupService(UvaTeaDbContext context)
    {
        _context = context;
    }

    // ==================== DESIGNATIONS ====================
    
    public async Task<List<DesignationDto>> GetDesignationsAsync()
    {
        var designations = await _context.Designations
            .AsNoTracking()
            .ToListAsync();

        return designations.Adapt<List<DesignationDto>>();
    }

    public async Task<Designation> GetDesignationByNameAsync(string name)
    {
        var designation = await _context.Designations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (designation == null)
        {
            throw new ResourceNotFoundException($"Designation '{name}' not found");
        }

        return designation;
    }

    public async Task<Designation> GetDesignationByIdAsync(int id)
    {
        var designation = await _context.Designations
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (designation == null)
        {
            throw new ResourceNotFoundException("Designation not found");
        }

        return designation;
    }

    // ==================== EMPLOYEE STATUSES ====================

    public async Task<List<EmployeeStatusDto>> GetEmployeeStatusesAsync()
    {
        var statuses = await _context.EmployeeStatuses
            .AsNoTracking()
            .ToListAsync();

        return statuses.Adapt<List<EmployeeStatusDto>>();
    }

    public async Task<Employeestatus> GetEmployeeStatusByNameAsync(string name)
    {
        var status = await _context.EmployeeStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (status == null)
        {
            throw new ResourceNotFoundException($"Employee status '{name}' not found");
        }

        return status;
    }

    public async Task<Employeestatus> GetEmployeeStatusByIdAsync(int id)
    {
        var status = await _context.EmployeeStatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (status == null)
        {
            throw new ResourceNotFoundException("Employee status not found");
        }

        return status;
    }

    // ==================== GENDERS ====================

    public async Task<List<GenderDto>> GetGendersAsync()
    {
        var genders = await _context.Genders
            .AsNoTracking()
            .ToListAsync();

        return genders.Adapt<List<GenderDto>>();
    }

    public async Task<Gender> GetGenderByNameAsync(string name)
    {
        var gender = await _context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (gender == null)
        {
            throw new ResourceNotFoundException($"Gender '{name}' not found");
        }

        return gender;
    }

    public async Task<Gender> GetGenderByIdAsync(int id)
    {
        var gender = await _context.Genders
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (gender == null)
        {
            throw new ResourceNotFoundException("Gender not found");
        }

        return gender;
    }
}