using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Dtos;

public record GreenLeafIntakeResponseDto(
    int Id,
    string BatchNumber,
    AreaDto? Area,
    ContainerTypeDto? ContainerType,
    WeatherConditionDto? WeatherCondition,
    LeafConditionDto? LeafCondition,
    LeafBatchStatusDto? LeafBatchStatus,
    DateTime PluckingDateTime,
    DateTime CollectionDateTime,
    DateTime ArrivalDateTime,
    decimal Weight,
    string? Remarks,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
