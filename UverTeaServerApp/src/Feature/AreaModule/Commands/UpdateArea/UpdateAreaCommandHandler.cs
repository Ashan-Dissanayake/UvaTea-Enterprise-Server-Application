using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.AreaModule.Models.Dtos;

namespace UverTeaServerApp.AreaModule.Commands.UpdateArea;

public class UpdateAreaCommandHandler : IRequestHandler<UpdateAreaCommand, AreaDetailResponseDto>
{
    private readonly UvaTeaDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAreaCommandHandler(
        UvaTeaDbContext context,
        IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AreaDetailResponseDto> Handle(
        UpdateAreaCommand request,
        CancellationToken cancellationToken)
    {
        var area = await _context.Areas
            .SingleOrDefaultAsync(
                a => a.Id == request.Id,
                cancellationToken);

        if (area == null)
        {
            throw new KeyNotFoundException(
                $"Area with ID '{request.Id}' not found.");
        }

        area.Acres = request.Acres;
        area.PlantCount = request.Plantcount;
        area.DoAttached = request.Doattached;
        area.DoProofing = request.Doproofing;
        area.SupervisorId = request.SupervisorId;
        area.AreaCategoryId = request.AreacategoryId;
        area.PlantingConfigurationId =
            request.PlantingConfigurationId;

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
                a => a.Id == request.Id,
                cancellationToken);

        return updatedArea.Adapt<AreaDetailResponseDto>();
    }
}