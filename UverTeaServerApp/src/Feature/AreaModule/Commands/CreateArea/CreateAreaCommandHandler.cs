using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.AreaModule.Commands.CreateArea;

public class CreateAreaCommandHandler
    : IRequestHandler<CreateAreaCommand, AreaDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public CreateAreaCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<AreaDetailResponseDto> Handle(
        CreateAreaCommand request,
        CancellationToken cancellationToken)
    {
        // Server-controlled default Area Status
        var activeStatus = await _context.Areastatuses
            .AsNoTracking()
            .SingleAsync(
                s => s.Name == "Active",
                cancellationToken);

        // Server-controlled default Growth Stage
        var newlyPlantedStage = await _context.Growthstages
            .AsNoTracking()
            .SingleAsync(
                g => g.Code == "NEWLY_PLANTED",
                cancellationToken);

        var area = request.Adapt<Area>();

        // Normalize Area Code
        area.Code = request.Code.Trim().ToUpperInvariant();

        // Server-controlled values
        area.AreaStatusId = activeStatus.Id;
        area.GrowthStageId = newlyPlantedStage.Id;
        area.UserId = _currentUser.UserId;

        _context.Areas.Add(area);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Reload navigation properties for response
        var createdArea = await _context.Areas
            .AsNoTracking()
            .Include(a => a.Supervisor)
            .Include(a => a.AreaStatus)
            .Include(a => a.AreaCategory)
            .Include(a => a.GrowthStage)
            .Include(a => a.PlantingConfiguration)
            .SingleAsync(
                a => a.Id == area.Id,
                cancellationToken);

        return createdArea.Adapt<AreaDetailResponseDto>();
    }
}