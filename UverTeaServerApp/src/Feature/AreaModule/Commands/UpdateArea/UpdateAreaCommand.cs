using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.AreaModule.Commands.UpdateArea;

public record UpdateAreaCommand(

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Area ID is required."
    )]
    int Id,

    [Range(
        typeof(decimal),
        "0.01",
        "99999.99",
        ErrorMessage = "Acres must be greater than 0."
    )]
    decimal Acres,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Plant count must be at least 1."
    )]
    int Plantcount,

    DateOnly? Doattached,

    DateOnly? Doproofing,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Supervisor ID is required."
    )]
    int? SupervisorId,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Area Category ID is required."
    )]
    int AreacategoryId,

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "A valid Planting Configuration ID is required."
    )]
    int PlantingConfigurationId,

    byte[] RowVersion

) : IRequest<AreaDetailResponseDto>, ITransactionalRequest;