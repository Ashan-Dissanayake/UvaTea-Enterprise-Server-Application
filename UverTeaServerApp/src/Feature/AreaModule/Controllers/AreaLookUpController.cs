using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Services;

namespace UverTeaServerApp.src.Feature.AreaModule.Controllers;

/// <summary>
/// Lookup master data endpoints for area statuses, categories,
/// growth stages, and planting configurations.
/// </summary>
[ApiController]
[Route("api/area-lookups")]
[Tags("Area Master Lookups")]
[Authorize]
public class AreaLookUpController : ControllerBase
{
    private readonly AreaLookupService _lookupService;

    public AreaLookUpController(
        AreaLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    // ==================== AREA STATUSES ====================

    /// <summary>
    /// Retrieves all area statuses.
    /// </summary>
    /// <response code="200">
    /// Returns all area statuses.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("statuses")]
    [ProducesResponseType(
        typeof(List<AreaStatusDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AreaStatusDto>>>
        GetAllStatuses()
    {
        var statuses =
            await _lookupService.GetAreaStatusesAsync();

        return Ok(statuses);
    }

    // ==================== AREA CATEGORIES ====================

    /// <summary>
    /// Retrieves all area categories.
    /// </summary>
    /// <response code="200">
    /// Returns all area categories.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("categories")]
    [ProducesResponseType(
        typeof(List<AreaCategoryDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<AreaCategoryDto>>>
        GetAllCategories()
    {
        var categories =
            await _lookupService.GetAreaCategoriesAsync();

        return Ok(categories);
    }

    // ==================== GROWTH STAGES ====================

    /// <summary>
    /// Retrieves all active growth stages.
    /// </summary>
    /// <response code="200">
    /// Returns all active growth stages.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("growth-stages")]
    [ProducesResponseType(
        typeof(List<GrowthStageDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<GrowthStageDto>>>
        GetAllGrowthStages()
    {
        var growthStages =
            await _lookupService.GetGrowthStagesAsync();

        return Ok(growthStages);
    }

    // ==================== PLANTING CONFIGURATIONS ====================

    /// <summary>
    /// Retrieves all active planting configurations.
    /// </summary>
    /// <response code="200">
    /// Returns all active planting configurations.
    /// </response>
    /// <response code="401">
    /// Unauthorized - valid JWT required.
    /// </response>
    [HttpGet("planting-configurations")]
    [ProducesResponseType(
        typeof(List<PlantingConfigurationDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<List<PlantingConfigurationDto>>>
        GetAllPlantingConfigurations()
    {
        var configurations =
            await _lookupService
                .GetPlantingConfigurationsAsync();

        return Ok(configurations);
    }
}