using Mapster;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;
using UverTeaServerApp.src.Feature.EmployeeModule.Services;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Controllers;

/// <summary>
/// Lookup master data endpoints for employee statuses, designations, and genders.
/// </summary>
[ApiController]
[Route("api/employee-lookups")] 
[Tags("Employee Master Lookups")]
public class EmployeeLookupController : ControllerBase
{
   private readonly EmployeeLookupService _lookupService;

    public EmployeeLookupController(EmployeeLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    // ==================== EMPLOYEE STATUSES ====================

    /// <summary>
    /// Retrieves all active employee statuses.
    /// </summary>
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(List<EmployeeStatusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<EmployeeStatusDto>>> GetAllStatuses()
    {
        var statuses = await _lookupService.GetEmployeeStatusesAsync();
        return Ok(statuses);
    }

    /// <summary>
    /// Retrieves an employee status by ID.
    /// </summary>
    [HttpGet("statuses/id/{id:int}")]
    [ProducesResponseType(typeof(EmployeeStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeStatusDto>> GetStatusById(int id)
    {
        var status = await _lookupService.GetEmployeeStatusByIdAsync(id);
        var statusDto = status.Adapt<EmployeeStatusDto>();
        return Ok(statusDto);
    }

    /// <summary>
    /// Retrieves an employee status by its unique name.
    /// </summary>
    [HttpGet("statuses/name/{name}")]
    [ProducesResponseType(typeof(EmployeeStatusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeStatusDto>> GetStatusByName(string name)
    {
        var status = await _lookupService.GetEmployeeStatusByNameAsync(name);
        var statusDto = status.Adapt<EmployeeStatusDto>();
        return Ok(statusDto);
    }

    // ==================== DESIGNATIONS ====================

    /// <summary>
    /// Retrieves all employee designations (roles/job titles).
    /// </summary>
    [HttpGet("designations")]
    [ProducesResponseType(typeof(List<DesignationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DesignationDto>>> GetAllDesignations()
    {
        var designations = await _lookupService.GetDesignationsAsync();
        return Ok(designations);
    }

    /// <summary>
    /// Retrieves an employee designation by ID.
    /// </summary>
    [HttpGet("designations/id/{id:int}")]
    [ProducesResponseType(typeof(DesignationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DesignationDto>> GetDesignationById(int id)
    {
        var designation = await _lookupService.GetDesignationByIdAsync(id);
        var designationDto = designation.Adapt<DesignationDto>();
        return Ok(designationDto);
    }

    /// <summary>
    /// Retrieves an employee designation by name.
    /// </summary>
    [HttpGet("designations/name/{name}")]
    [ProducesResponseType(typeof(DesignationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DesignationDto>> GetDesignationByName(string name)
    {
        var designation = await _lookupService.GetDesignationByNameAsync(name);
        var designationDto = designation.Adapt<DesignationDto>();
        return Ok(designationDto);
    }

    // ==================== GENDERS ====================

    /// <summary>
    /// Retrieves all gender types.
    /// </summary>
    [HttpGet("genders")]
    [ProducesResponseType(typeof(List<GenderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GenderDto>>> GetAllGenders()
    {
        var genders = await _lookupService.GetGendersAsync();
        return Ok(genders);
    }

    /// <summary>
    /// Retrieves a gender type by ID.
    /// </summary>
    [HttpGet("genders/id/{id:int}")]
    [ProducesResponseType(typeof(GenderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenderDto>> GetGenderById(int id)
    {
        var gender = await _lookupService.GetGenderByIdAsync(id);
        var genderDto = gender.Adapt<GenderDto>();
        return Ok(genderDto);
    }

    /// <summary>
    /// Retrieves a gender type by name.
    /// </summary>
    [HttpGet("genders/name/{name}")]
    [ProducesResponseType(typeof(GenderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenderDto>> GetGenderByName(string name)
    {
        var gender = await _lookupService.GetGenderByNameAsync(name);
        var genderDto = gender.Adapt<GenderDto>();
        return Ok(genderDto);
    }
}