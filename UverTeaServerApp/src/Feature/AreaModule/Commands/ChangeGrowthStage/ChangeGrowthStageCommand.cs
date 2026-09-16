using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.AreaModule.Commands.ChangeGrowthStage;

public record ChangeGrowthStageCommand(

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Area ID is required."
    )]
    int AreaId,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Growth Stage ID is required."
    )]
    int GrowthStageId,

    [MaxLength(
        255,
        ErrorMessage = "Reason cannot exceed 255 characters."
    )]
    string? Reason,
    
    byte[] RowVersion

) : IRequest<AreaDetailResponseDto>, ITransactionalRequest;