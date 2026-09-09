using UverTeaServerApp.src.Feature.EmployeeModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

public record AreaDetailResponseDto(
    int Id,
    string? Code,
    decimal? Acres,
    DateOnly? Doattached,
    int? Plantcount,
    DateOnly? Doproofing,
    EmployeeSummaryDto? Supervisor,
    AreaStatusDto Areastatus,
    AreaCategoryDto Areacategory
);