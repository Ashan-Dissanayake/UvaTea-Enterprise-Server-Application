using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DeactivatePlantingConfiguration;

public record DeactivatePlantingConfigurationCommand(
    [Range(1, int.MaxValue,
        ErrorMessage = "A valid Planting Configuration ID is required.")]
    int Id
) : IRequest<PlantingConfigurationDto>, ITransactionalRequest;