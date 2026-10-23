
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;

public sealed class CompleteGreenLeafProcessingCommandHandlerTests
{
    private static async Task<(
        UvaTeaDbContext Context,
        Mock<IUnitOfWork> UnitOfWork,
        CompleteGreenLeafProcessingCommandHandler Handler)>
        CreateSutAsync(
            string batchStatus = "PROCESSING",
            bool includeBatch = true,
            bool includeProcessedStatus = true,
            bool includeProcessingRecord = false)
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UvaTeaDbContext(options);

        var areaStatus = new Areastatus { Id = 1, Name = "ACTIVE" };
        var area = new Area
        {
            Id = 1,
            AreaStatusId = 1,
            AreaStatus = areaStatus,
            AreaCategoryId = 1,
            UserId = 1
        };

        context.Areastatuses.Add(areaStatus);
        context.Areas.Add(area);
        context.Containertypes.Add(
            new Containertype { Id = 1, Name = "Standard" });
        context.Weatherconditions.Add(
            new Weathercondition { Id = 1, Name = "Sunny" });
        context.Leafconditions.Add(
            new Leafcondition { Id = 1, Name = "FRESH" });

        var initialStatus = new Leafbatchstatus
        {
            Id = 1,
            Name = batchStatus
        };

        context.Leafbatchstatuses.Add(initialStatus);

        if (includeProcessedStatus && batchStatus != "PROCESSED")
        {
            context.Leafbatchstatuses.Add(
                new Leafbatchstatus { Id = 2, Name = "PROCESSED" });
        }

        if (includeBatch)
        {
            var batch = new Greenleafbatch
            {
                Id = 1,
                BatchNumber = "GLB-001",
                AreaId = 1,
                Area = area,
                ContainerTypeId = 1,
                ContainerType = context.Containertypes.Local.Single(),
                WeatherConditionId = 1,
                WeatherCondition = context.Weatherconditions.Local.Single(),
                LeafConditionId = 1,
                LeafCondition = context.Leafconditions.Local.Single(),
                LeafBatchStatusId = 1,
                LeafBatchStatus = initialStatus,
                PluckingDateTime = DateTime.UtcNow.AddHours(-3),
                CollectionDateTime = DateTime.UtcNow.AddHours(-2),
                ArrivalDateTime = DateTime.UtcNow.AddHours(-1),
                Weight = 100m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Greenleafbatches.Add(batch);

            if (includeProcessingRecord)
            {
                context.Greenleafprocessingrecords.Add(
                    new Greenleafprocessingrecord
                    {
                        Id = 1,
                        GreenLeafBatchId = 1,
                        GreenLeafBatch = batch,
                        ProcessedWeight = 72.5m,
                        ProcessingNotes = "Existing record",
                        CompletedAt = DateTime.UtcNow
                    });
            }
        }

        await context.SaveChangesAsync();

        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns<CancellationToken>(
                token => context.SaveChangesAsync(token));

        var handler = new CompleteGreenLeafProcessingCommandHandler(
            context,
            unitOfWork.Object);

        return (context, unitOfWork, handler);
    }

    [Fact]
    public async Task Handle_ValidRequest_CompletesBatchAndCreatesRecord()
    {
        var (context, unitOfWork, handler) = await CreateSutAsync();

        var response = await handler.Handle(
            new CompleteGreenLeafProcessingCommand(
                1, 72.5m, "Normal processing"),
            CancellationToken.None);

        response.GreenLeafBatchId.Should().Be(1);
        response.BatchNumber.Should().Be("GLB-001");
        response.ProcessedWeight.Should().Be(72.5m);
        response.ProcessingNotes.Should().Be("Normal processing");
        response.Status.Should().Be("PROCESSED");

        var batch = await context.Greenleafbatches.SingleAsync();
        batch.LeafBatchStatus.Name.Should().Be("PROCESSED");

        var record = await context.Greenleafprocessingrecords.SingleAsync();
        record.ProcessedWeight.Should().Be(72.5m);
        record.ProcessingNotes.Should().Be("Normal processing");
        record.CompletedAt.Should().Be(response.CompletedAt);

        unitOfWork.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_MissingBatch_ThrowsKeyNotFoundException()
    {
        var (_, _, handler) = await CreateSutAsync(includeBatch: false);

        var act = () => handler.Handle(
            new CompleteGreenLeafProcessingCommand(99, 50m, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Theory]
    [InlineData("RECEIVED")]
    [InlineData("QUEUED")]
    [InlineData("PROCESSED")]
    public async Task Handle_BatchNotProcessing_ThrowsInvalidOperationException(
        string status)
    {
        var (_, _, handler) = await CreateSutAsync(batchStatus: status);

        var act = () => handler.Handle(
            new CompleteGreenLeafProcessingCommand(1, 50m, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*PROCESSING status*");
    }

    [Fact]
    public async Task Handle_ProcessedWeightExceedsReceivedWeight_Throws()
    {
        var (_, _, handler) = await CreateSutAsync();

        var act = () => handler.Handle(
            new CompleteGreenLeafProcessingCommand(1, 101m, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*cannot exceed*");
    }

    [Fact]
    public async Task Handle_ExistingProcessingRecord_ThrowsDuplicateCompletion()
    {
        var (_, _, handler) = await CreateSutAsync(
            includeProcessingRecord: true);

        var act = () => handler.Handle(
            new CompleteGreenLeafProcessingCommand(1, 70m, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*already been completed*");
    }

    [Fact]
    public async Task Handle_MissingProcessedStatus_Throws()
    {
        var (_, _, handler) = await CreateSutAsync(
            includeProcessedStatus: false);

        var act = () => handler.Handle(
            new CompleteGreenLeafProcessingCommand(1, 70m, null),
            CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*PROCESSED status is not configured*");
    }
}
