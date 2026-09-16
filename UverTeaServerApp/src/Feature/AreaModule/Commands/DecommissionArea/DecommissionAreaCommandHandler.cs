using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Commands.DecommissionArea;

public class DecommissionAreaCommandHandler
    : IRequestHandler<DecommissionAreaCommand, AreaDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DecommissionAreaCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<AreaDetailResponseDto> Handle(
        DecommissionAreaCommand request,
        CancellationToken cancellationToken)
    {
        var area = await _context.Areas
            .SingleOrDefaultAsync(
                a => a.Id == request.AreaId,
                cancellationToken);

        if (area == null)
        {
            throw new ResourceNotFoundException(
                $"Area with ID '{request.AreaId}' not found.");
        }

        var currentStatusId = area.AreaStatusId;

        var decommissionedStatus = await _context.Areastatuses
            .SingleOrDefaultAsync(
                s => s.Name == "Decommissioned",
                cancellationToken);

        if (decommissionedStatus == null)
        {
            throw new ResourceNotFoundException(
                "Decommissioned area status not found.");
        }

        /*
         * Use the RowVersion received from the client
         * as the original concurrency value.
         */
        _context.Entry(area)
            .Property(a => a.RowVersion)
            .OriginalValue = request.RowVersion;

        area.AreaStatusId = decommissionedStatus.Id;

        var history = new Areastatushistory
        {
            Area_id = area.Id,
            From_status_id = currentStatusId,
            To_status_id = decommissionedStatus.Id,
            Changedat = DateTime.UtcNow,
            Changedby = _currentUser.UserId,
            Reason = string.IsNullOrWhiteSpace(request.Reason)
                ? null
                : request.Reason.Trim()
        };

        _context.Areastatushistories.Add(history);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

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