using System.ComponentModel.DataAnnotations;
using MediatR;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DecommissionArea;

public record DecommissionAreaCommand(
    [Range(1, int.MaxValue, ErrorMessage = "A valid Area ID is required.")]
    int AreaId,

    [MaxLength(255, ErrorMessage = "Reason cannot exceed 255 characters.")]
    string? Reason,
    
    byte[] RowVersion
) : IRequest<AreaDetailResponseDto>, ITransactionalRequest;