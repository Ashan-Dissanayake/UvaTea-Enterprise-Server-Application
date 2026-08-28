using Mapster;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Services;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Controllers;

[ApiController]
[Route("/employee-lookups")] 
public class EmployeeLookupController : ControllerBase
{
   private readonly EmployeeLookupService _lookupService;

    public EmployeeLookupController(EmployeeLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    // ==================== EMPLOYEE STATUSES ====================

    [HttpGet("statuses")]
    public async Task<ActionResult<List<EmployeeStatusDto>>> GetAllStatuses()
    {
        var statuses = await _lookupService.GetEmployeeStatusesAsync();
        return Ok(statuses);
    }

    [HttpGet("statuses/id/{id:int}")]
    public async Task<ActionResult<EmployeeStatusDto>> GetStatusById(int id)
    {
        var status = await _lookupService.GetEmployeeStatusByIdAsync(id);
        var statusDto = status.Adapt<EmployeeStatusDto>();
        return Ok(statusDto);
    }

    [HttpGet("statuses/name/{name}")]
    public async Task<ActionResult<EmployeeStatusDto>> GetStatusByName(string name)
    {
        var status = await _lookupService.GetEmployeeStatusByNameAsync(name);
        var statusDto = status.Adapt<EmployeeStatusDto>();
        return Ok(statusDto);
    }

    // ==================== DESIGNATIONS ====================

    [HttpGet("designations")]
    public async Task<ActionResult<List<DesignationDto>>> GetAllDesignations()
    {
        var designations = await _lookupService.GetDesignationsAsync();
        return Ok(designations);
    }

    [HttpGet("designations/id/{id:int}")]
    public async Task<ActionResult<DesignationDto>> GetDesignationById(int id)
    {
        var designation = await _lookupService.GetDesignationByIdAsync(id);
        var designationDto = designation.Adapt<DesignationDto>();
        return Ok(designationDto);
    }

    [HttpGet("designations/name/{name}")]
    public async Task<ActionResult<DesignationDto>> GetDesignationByName(string name)
    {
        var designation = await _lookupService.GetDesignationByNameAsync(name);
        var designationDto = designation.Adapt<DesignationDto>();
        return Ok(designationDto);
    }

    // ==================== GENDERS ====================

    [HttpGet("genders")]
    public async Task<ActionResult<List<GenderDto>>> GetAllGenders()
    {
        var genders = await _lookupService.GetGendersAsync();
        return Ok(genders);
    }

    [HttpGet("genders/id/{id:int}")]
    public async Task<ActionResult<GenderDto>> GetGenderById(int id)
    {
        var gender = await _lookupService.GetGenderByIdAsync(id);
        var genderDto = gender.Adapt<GenderDto>();
        return Ok(genderDto);
    }

    [HttpGet("genders/name/{name}")]
    public async Task<ActionResult<GenderDto>> GetGenderByName(string name)
    {
        var gender = await _lookupService.GetGenderByNameAsync(name);
        var genderDto = gender.Adapt<GenderDto>();
        return Ok(genderDto);
    }
}