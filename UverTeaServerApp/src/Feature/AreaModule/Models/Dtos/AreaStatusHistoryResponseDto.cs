namespace UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

public record AreaStatusHistoryResponseDto(
    int Id,
    int AreaId,
    AreaStatusDto FromStatus,
    AreaStatusDto ToStatus,
    DateTime ChangedAt,
    int ChangedBy,
    string? Reason
);