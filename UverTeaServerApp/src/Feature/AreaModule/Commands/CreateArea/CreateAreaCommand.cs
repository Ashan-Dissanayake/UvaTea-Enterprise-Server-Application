using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.AreaModule.Commands.CreateArea;

public record CreateAreaCommand(

    [Required(ErrorMessage = "Area code is required.")]
    [RegularExpression(
        @"^[MF]\d{4}$",
        ErrorMessage = "Invalid area code."
    )]
    string Code,

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
    int PlantingConfigurationId

) : IRequest<AreaDetailResponseDto>, ITransactionalRequest;