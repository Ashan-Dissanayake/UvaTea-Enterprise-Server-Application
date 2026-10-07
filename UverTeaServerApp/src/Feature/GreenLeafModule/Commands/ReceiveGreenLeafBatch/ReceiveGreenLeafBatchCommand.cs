using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Dtos;

namespace UverTeaServerApp.GreenLeafModule.Commands.ReceiveGreenLeafBatch;

public record ReceiveGreenLeafBatchCommand(

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Area ID is required."
    )]
    int AreaId,

    DateTime PluckingDateTime,

    DateTime CollectionDateTime,

    DateTime ArrivalDateTime,

    [Range(
        0.01,
        double.MaxValue,
        ErrorMessage = "Weight must be greater than zero."
    )]
    decimal Weight,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Container Type ID is required."
    )]
    int ContainerTypeId,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Weather Condition ID is required."
    )]
    int WeatherConditionId,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Leaf Condition ID is required."
    )]
    int LeafConditionId,

    [MaxLength(
        500,
        ErrorMessage = "Remarks cannot exceed 500 characters."
    )]
    string? Remarks

) : IRequest<GreenLeafIntakeResponseDto>, ITransactionalRequest;