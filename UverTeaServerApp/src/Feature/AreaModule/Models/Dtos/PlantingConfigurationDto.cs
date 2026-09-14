namespace UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

public record PlantingConfigurationDto(
    int Id,
    string Code,
    string Name,
    decimal Rowspacingfeet,
    decimal Plantspacingfeet
);