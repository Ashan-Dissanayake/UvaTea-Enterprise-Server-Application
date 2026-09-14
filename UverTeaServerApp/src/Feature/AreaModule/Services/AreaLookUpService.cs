using Mapster;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Services;

public class AreaLookupService
{
    private readonly UvaTeaDbContext _context;

    public AreaLookupService(UvaTeaDbContext context)
    {
        _context = context;
    }

    // ==================== AREA STATUSES ====================

    public async Task<List<AreaStatusDto>> GetAreaStatusesAsync()
    {
        var areastatuses = await _context.Areastatuses
            .AsNoTracking()
            .ToListAsync();

        return areastatuses.Adapt<List<AreaStatusDto>>();
    }

    public async Task<Areastatus> GetAreaStatusByNameAsync(string name)
    {
        var areastatus = await _context.Areastatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Name == name);

        if (areastatus == null)
        {
            throw new ResourceNotFoundException(
                $"Area Status '{name}' not found");
        }

        return areastatus;
    }

    public async Task<Areastatus> GetAreaStatusByIdAsync(int id)
    {
        var areastatus = await _context.Areastatuses
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (areastatus == null)
        {
            throw new ResourceNotFoundException(
                "Area Status not found");
        }

        return areastatus;
    }

    // ==================== AREA CATEGORIES ====================

    public async Task<List<AreaCategoryDto>> GetAreaCategoriesAsync()
    {
        var categories = await _context.Areacategories
            .AsNoTracking()
            .ToListAsync();

        return categories.Adapt<List<AreaCategoryDto>>();
    }

    public async Task<Areacategory> GetAreaCategoryByNameAsync(string name)
    {
        var category = await _context.Areacategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);

        if (category == null)
        {
            throw new ResourceNotFoundException(
                $"Area category '{name}' not found");
        }

        return category;
    }

    public async Task<Areacategory> GetAreaCategoryByIdAsync(int id)
    {
        var category = await _context.Areacategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            throw new ResourceNotFoundException(
                "Area category not found");
        }

        return category;
    }

    // ==================== GROWTH STAGES ====================

    public async Task<List<GrowthStageDto>> GetGrowthStagesAsync()
    {
        var growthStages = await _context.Growthstages
            .AsNoTracking()
            .Where(g => g.Isactive)
            .OrderBy(g => g.Displayorder)
            .ToListAsync();

        return growthStages.Adapt<List<GrowthStageDto>>();
    }

    public async Task<Growthstage> GetGrowthStageByNameAsync(string name)
    {
        var growthStage = await _context.Growthstages
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Name == name);

        if (growthStage == null)
        {
            throw new ResourceNotFoundException(
                $"Growth stage '{name}' not found");
        }

        return growthStage;
    }

    public async Task<Growthstage> GetGrowthStageByIdAsync(int id)
    {
        var growthStage = await _context.Growthstages
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == id);

        if (growthStage == null)
        {
            throw new ResourceNotFoundException(
                "Growth stage not found");
        }

        return growthStage;
    }

    // ==================== PLANTING CONFIGURATIONS ====================

    public async Task<List<PlantingConfigurationDto>> GetPlantingConfigurationsAsync()
    {
        var configurations = await _context.Plantingconfigurations
            .AsNoTracking()
            .Where(p => p.Isactive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return configurations.Adapt<List<PlantingConfigurationDto>>();
    }

    public async Task<Plantingconfiguration> GetPlantingConfigurationByNameAsync(
        string name)
    {
        var configuration = await _context.Plantingconfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);

        if (configuration == null)
        {
            throw new ResourceNotFoundException(
                $"Planting configuration '{name}' not found");
        }

        return configuration;
    }

    public async Task<Plantingconfiguration> GetPlantingConfigurationByIdAsync(
        int id)
    {
        var configuration = await _context.Plantingconfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (configuration == null)
        {
            throw new ResourceNotFoundException(
                "Planting configuration not found");
        }

        return configuration;
    }
}