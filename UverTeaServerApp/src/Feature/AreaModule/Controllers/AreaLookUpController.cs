using Microsoft.AspNetCore.Mvc;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Services;

namespace UverTeaServerApp.src.Feature.AreaModule.Controllers;

[ApiController]
[Route("api/area-lookups")]
[Tags("Area Master Lookups")]
public class AreaLookUpController : ControllerBase
{

    private readonly AreaLookupService _lookupService;

    public AreaLookUpController(AreaLookupService lookupService)
    {
        _lookupService = lookupService;
    }

    /// <summary>
    /// Retrieves all  area statuses.
    /// </summary>
    [HttpGet("statuses")]
    [ProducesResponseType(typeof(List<AreaStatusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AreaStatusDto>>> GetAllStatuses()
    {
        var statuses = await _lookupService.GetAreaStatusesAsync();
        return Ok(statuses);
    }

    /// <summary>
    /// Retrieves all  area categories.
    /// </summary>
    [HttpGet("categories")]
    [ProducesResponseType(typeof(List<AreaCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AreaCategoryDto>>> GetAllCategories()
    {
        var categories = await _lookupService.GetAreaCategoriesAsync();
        return Ok(categories);
    }

    /// <summary>
    /// Retrieves all  growthstages
    /// </summary>
    [HttpGet("growth-stages")]
    [ProducesResponseType(typeof(List<GrowthStageDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<GrowthStageDto>>> GetAllGrowthStages()
    {
        var growthStages = await _lookupService.GetGrowthStagesAsync();
        return Ok(growthStages);
    }


    /// <summary> 
    /// Retrieves all active planting configurations. 
    ///</summary> 
    [HttpGet("planting-configurations")]
    [ProducesResponseType(typeof(List<PlantingConfigurationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PlantingConfigurationDto>>> GetAllPlantingConfigurations()
    {
        var configurations = await _lookupService.GetPlantingConfigurationsAsync();
        return Ok(configurations);
    }
}