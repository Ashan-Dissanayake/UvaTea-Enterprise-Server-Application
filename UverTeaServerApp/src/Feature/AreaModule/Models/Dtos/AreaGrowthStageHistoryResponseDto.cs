namespace UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

public record AreaGrowthStageHistoryResponseDto(
    int Id,
    int AreaId,
    GrowthStageDto FromGrowthStage,
    GrowthStageDto ToGrowthStage,
    DateTime ChangedAt,
    int ChangedBy,
    string? Reason
);