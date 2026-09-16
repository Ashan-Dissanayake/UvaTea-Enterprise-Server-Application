using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DeactivatePlantingConfiguration;

public class DeactivatePlantingConfigurationCommandHandler
    : IRequestHandler<
        DeactivatePlantingConfigurationCommand,
        PlantingConfigurationDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivatePlantingConfigurationCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<PlantingConfigurationDto> Handle(
        DeactivatePlantingConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        var configuration = await _context.Plantingconfigurations
            .SingleOrDefaultAsync(
                p => p.Id == request.Id,
                cancellationToken);

        if (configuration == null)
        {
            throw new ResourceNotFoundException(
                $"Planting Configuration with ID '{request.Id}' not found.");
        }

        configuration.Isactive = false;
        configuration.Updatedat = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return configuration.Adapt<PlantingConfigurationDto>();
    }
}