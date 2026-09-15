using Mapster;
using Microsoft.AspNetCore.Authorization;
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
[Authorize]
public class EmployeeLookupController : ControllerBase
{
    private readonly EmployeeLookupService _lookupService;

    public EmployeeLookupController(
        EmployeeLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    // ==================== EMPLOYEE STATUSES ====================

    /// <summary>
    /// Retrieves all active employee statuses.
    /// </summary>
    /// <response code="200">
    /// Returns all active employee statuses.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("statuses")]
    [ProducesResponseType(
        typeof(List<EmployeeStatusDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<EmployeeStatusDto>>>
        GetAllStatuses()
    {
        var statuses =
            await _lookupService.GetEmployeeStatusesAsync();

        return Ok(statuses);
    }

    /// <summary>
    /// Retrieves an employee status by ID.
    /// </summary>
    /// <param name="id">Employee status ID.</param>
    /// <response code="200">
    /// Returns the requested employee status.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Employee status not found.
    /// </response>
    [HttpGet("statuses/id/{id:int}")]
    [ProducesResponseType(
        typeof(EmployeeStatusDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeStatusDto>>
        GetStatusById(int id)
    {
        var status =
            await _lookupService
                .GetEmployeeStatusByIdAsync(id);

        return Ok(status.Adapt<EmployeeStatusDto>());
    }

    /// <summary>
    /// Retrieves an employee status by its unique name.
    /// </summary>
    /// <param name="name">Employee status name.</param>
    /// <response code="200">
    /// Returns the requested employee status.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Employee status not found.
    /// </response>
    [HttpGet("statuses/name/{name}")]
    [ProducesResponseType(
        typeof(EmployeeStatusDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmployeeStatusDto>>
        GetStatusByName(string name)
    {
        var status =
            await _lookupService
                .GetEmployeeStatusByNameAsync(name);

        return Ok(status.Adapt<EmployeeStatusDto>());
    }

    // ==================== DESIGNATIONS ====================

    /// <summary>
    /// Retrieves all employee designations.
    /// </summary>
    /// <response code="200">
    /// Returns all employee designations.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("designations")]
    [ProducesResponseType(
        typeof(List<DesignationDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<DesignationDto>>>
        GetAllDesignations()
    {
        var designations =
            await _lookupService.GetDesignationsAsync();

        return Ok(designations);
    }

    /// <summary>
    /// Retrieves an employee designation by ID.
    /// </summary>
    /// <param name="id">Employee designation ID.</param>
    /// <response code="200">
    /// Returns the requested employee designation.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Employee designation not found.
    /// </response>
    [HttpGet("designations/id/{id:int}")]
    [ProducesResponseType(
        typeof(DesignationDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DesignationDto>>
        GetDesignationById(int id)
    {
        var designation =
            await _lookupService
                .GetDesignationByIdAsync(id);

        return Ok(designation.Adapt<DesignationDto>());
    }

    /// <summary>
    /// Retrieves an employee designation by name.
    /// </summary>
    /// <param name="name">Employee designation name.</param>
    /// <response code="200">
    /// Returns the requested employee designation.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Employee designation not found.
    /// </response>
    [HttpGet("designations/name/{name}")]
    [ProducesResponseType(
        typeof(DesignationDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DesignationDto>>
        GetDesignationByName(string name)
    {
        var designation =
            await _lookupService
                .GetDesignationByNameAsync(name);

        return Ok(designation.Adapt<DesignationDto>());
    }

    // ==================== GENDERS ====================

    /// <summary>
    /// Retrieves all gender types.
    /// </summary>
    /// <response code="200">
    /// Returns all gender types.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("genders")]
    [ProducesResponseType(
        typeof(List<GenderDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<GenderDto>>>
        GetAllGenders()
    {
        var genders =
            await _lookupService.GetGendersAsync();

        return Ok(genders);
    }

    /// <summary>
    /// Retrieves a gender type by ID.
    /// </summary>
    /// <param name="id">Gender ID.</param>
    /// <response code="200">
    /// Returns the requested gender type.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Gender type not found.
    /// </response>
    [HttpGet("genders/id/{id:int}")]
    [ProducesResponseType(
        typeof(GenderDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenderDto>>
        GetGenderById(int id)
    {
        var gender =
            await _lookupService
                .GetGenderByIdAsync(id);

        return Ok(gender.Adapt<GenderDto>());
    }

    /// <summary>
    /// Retrieves a gender type by name.
    /// </summary>
    /// <param name="name">Gender name.</param>
    /// <response code="200">
    /// Returns the requested gender type.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    /// <response code="404">
    /// Gender type not found.
    /// </response>
    [HttpGet("genders/name/{name}")]
    [ProducesResponseType(
        typeof(GenderDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GenderDto>>
        GetGenderByName(string name)
    {
        var gender =
            await _lookupService
                .GetGenderByNameAsync(name);

        return Ok(gender.Adapt<GenderDto>());
    }
}