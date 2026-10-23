using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Commands.StartGreenLeafProcessing;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Commands.StartGreenLeafProcessing;

public class StartGreenLeafProcessingCommandHandlerTests
{
    private static async Task<(
        UvaTeaDbContext Context,
        Mock<IUnitOfWork> UnitOfWork,
        StartGreenLeafProcessingCommandHandler Handler)>
        CreateContextAsync(
            string batchStatus = "QUEUED",
            bool includeRiskAssessment = true,
            bool includeProcessingStatus = true)
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UvaTeaDbContext(options);

        var areaStatus = new Areastatus
        {
            Id = 1,
            Name = "Active"
        };

        var area = new Area
        {
            Id = 1,
            AreaStatusId = 1,
            AreaStatus = areaStatus,
            AreaCategoryId = 1,
            UserId = 1
        };

        var containerType = new Containertype
        {
            Id = 1,
            Name = "Standard"
        };

        var weatherCondition = new Weathercondition
        {
            Id = 1,
            Name = "Sunny"
        };

        var leafCondition = new Leafcondition
        {
            Id = 1,
            Name = "FRESH"
        };

        var queuedStatus = new Leafbatchstatus
        {
            Id = 1,
            Name = batchStatus
        };

        context.Areastatuses.Add(areaStatus);
        context.Areas.Add(area);
        context.Containertypes.Add(containerType);
        context.Weatherconditions.Add(weatherCondition);
        context.Leafconditions.Add(leafCondition);
        context.Leafbatchstatuses.Add(queuedStatus);

        if (includeProcessingStatus)
        {
            context.Leafbatchstatuses.Add(new Leafbatchstatus
            {
                Id = 2,
                Name = "PROCESSING"
            });
        }

        await context.SaveChangesAsync();

        var batch = new Greenleafbatch
        {
            Id = 1,
            BatchNumber = "GL-TEST-001",
            AreaId = area.Id,
            Area = area,
            ContainerTypeId = containerType.Id,
            ContainerType = containerType,
            WeatherConditionId = weatherCondition.Id,
            WeatherCondition = weatherCondition,
            LeafConditionId = leafCondition.Id,
            LeafCondition = leafCondition,
            LeafBatchStatusId = queuedStatus.Id,
            LeafBatchStatus = queuedStatus,
            PluckingDateTime = new DateTime(2026, 10, 22, 8, 0, 0),
            CollectionDateTime = new DateTime(2026, 10, 22, 9, 0, 0),
            ArrivalDateTime = new DateTime(2026, 10, 22, 10, 0, 0),
            Weight = 100m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (includeRiskAssessment)
        {
            batch.RiskAssessment = new Greenleafriskassessment
            {
                Id = 1,
                GreenLeafBatchId = batch.Id,
                GreenLeafBatch = batch,
                RiskScore = 25,
                RiskLevel = "MEDIUM",
                RiskReason = "Test risk assessment",
                AssessedAt = DateTime.UtcNow
            };
        }

        context.Greenleafbatches.Add(batch);
        await context.SaveChangesAsync();

        var unitOfWork = new Mock<IUnitOfWork>();

        unitOfWork
            .Setup(x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()))
            .Returns((CancellationToken token) =>
                context.SaveChangesAsync(token));

        var handler = new StartGreenLeafProcessingCommandHandler(
            context,
            unitOfWork.Object);

        return (context, unitOfWork, handler);
    }

    [Fact]
    public async Task Handle_ShouldSetProcessingStatus_WhenBatchIsQueuedAndAssessed()
    {
        var (context, unitOfWork, handler) = await CreateContextAsync();

        try
        {
            var result = await handler.Handle(
                new StartGreenLeafProcessingCommand(1),
                CancellationToken.None);

            result.GreenLeafBatchId.Should().Be(1);
            result.BatchNumber.Should().Be("GL-TEST-001");
            result.Status.Should().Be("PROCESSING");

            var updatedBatch = await context.Greenleafbatches
                .Include(x => x.LeafBatchStatus)
                .SingleAsync(x => x.Id == 1);

            updatedBatch.LeafBatchStatus.Name.Should().Be("PROCESSING");

            unitOfWork.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowKeyNotFound_WhenBatchDoesNotExist()
    {
        var (context, unitOfWork, handler) = await CreateContextAsync();

        try
        {
            var act = () => handler.Handle(
                new StartGreenLeafProcessingCommand(999),
                CancellationToken.None);

            await act.Should().ThrowAsync<KeyNotFoundException>();

            unitOfWork.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperation_WhenBatchIsNotQueued()
    {
        var (context, unitOfWork, handler) = await CreateContextAsync(
            batchStatus: "RECEIVED");

        try
        {
            var act = () => handler.Handle(
                new StartGreenLeafProcessingCommand(1),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*Only queued*");

            unitOfWork.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperation_WhenRiskAssessmentIsMissing()
    {
        var (context, unitOfWork, handler) = await CreateContextAsync(
            includeRiskAssessment: false);

        try
        {
            var act = () => handler.Handle(
                new StartGreenLeafProcessingCommand(1),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*risk assessment*");

            unitOfWork.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperation_WhenProcessingStatusIsMissing()
    {
        var (context, unitOfWork, handler) = await CreateContextAsync(
            includeProcessingStatus: false);

        try
        {
            var act = () => handler.Handle(
                new StartGreenLeafProcessingCommand(1),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*PROCESSING status*");

            unitOfWork.Verify(
                x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperation_WhenConcurrencyConflictOccurs()
    {
        var (context, unitOfWork, _) = await CreateContextAsync();

        try
        {
            unitOfWork
                .Setup(x => x.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            var handler = new StartGreenLeafProcessingCommandHandler(
                context,
                unitOfWork.Object);

            var act = () => handler.Handle(
                new StartGreenLeafProcessingCommand(1),
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("*changed by another request*");
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}