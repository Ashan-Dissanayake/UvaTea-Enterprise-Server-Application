using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.AreaModule.Commands.ChangeGrowthStage;

public class ChangeGrowthStageCommandHandler
    : IRequestHandler<
        ChangeGrowthStageCommand,
        AreaDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public ChangeGrowthStageCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<AreaDetailResponseDto> Handle(
        ChangeGrowthStageCommand request,
        CancellationToken cancellationToken)
    {
        var area = await _context.Areas
            .SingleOrDefaultAsync(
                a => a.Id == request.AreaId,
                cancellationToken);

        if (area == null)
        {
            throw new KeyNotFoundException(
                $"Area with ID '{request.AreaId}' not found.");
        }

        var currentGrowthStageId =
            area.GrowthStageId
            ?? throw new InvalidOperationException(
                "The area does not have a current Growth Stage.");

        var requestedGrowthStage =
            await _context.Growthstages
                .SingleOrDefaultAsync(
                    g =>
                        g.Id == request.GrowthStageId &&
                        g.Isactive,
                    cancellationToken);

        if (requestedGrowthStage == null)
        {
            throw new KeyNotFoundException(
                $"Growth Stage with ID " +
                $"'{request.GrowthStageId}' not found.");
        }

        area.GrowthStageId =
            requestedGrowthStage.Id;

        var history = new Areagrowthstagehistory
        {
            Area_id = area.Id,

            From_growthstage_id =
                currentGrowthStageId,

            To_growthstage_id =
                requestedGrowthStage.Id,

            Changedat = DateTime.UtcNow,

            Changedby = _currentUser.UserId,

            Reason = string.IsNullOrWhiteSpace(request.Reason)
                ? null
                : request.Reason.Trim()
        };

        _context.Areagrowthstagehistories.Add(history);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        var updatedArea = await _context.Areas
            .AsNoTracking()
            .Include(a => a.Supervisor)
            .Include(a => a.AreaStatus)
            .Include(a => a.AreaCategory)
            .Include(a => a.GrowthStage)
            .Include(a => a.PlantingConfiguration)
            .SingleAsync(
                a => a.Id == request.AreaId,
                cancellationToken);

        return updatedArea.Adapt<AreaDetailResponseDto>();
    }
}