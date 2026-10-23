
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed class GetGreenLeafYieldAnalysisQueryHandlerTests
{
    private static async Task<(
        UvaTeaDbContext Context,
        GetGreenLeafYieldAnalysisQueryHandler Handler)>
        CreateSutAsync()
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UvaTeaDbContext(options);

        var areaStatus = new Areastatus
        {
            Id = 1,
            Name = "ACTIVE"
        };

        var area1 = new Area
        {
            Id = 1,
            AreaStatusId = 1,
            AreaStatus = areaStatus,
            AreaCategoryId = 1,
            UserId = 1
        };

        var area2 = new Area
        {
            Id = 2,
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

        var processedStatus = new Leafbatchstatus
        {
            Id = 1,
            Name = "PROCESSED"
        };

        var processingStatus = new Leafbatchstatus
        {
            Id = 2,
            Name = "PROCESSING"
        };

        context.Areastatuses.Add(areaStatus);
        context.Areas.AddRange(area1, area2);
        context.Containertypes.Add(containerType);
        context.Weatherconditions.Add(weatherCondition);
        context.Leafconditions.Add(leafCondition);
        context.Leafbatchstatuses.AddRange(
            processedStatus,
            processingStatus);

        await context.SaveChangesAsync();

        var handler = new GetGreenLeafYieldAnalysisQueryHandler(context);

        return (context, handler);
    }

    private static Greenleafbatch CreateBatch(
        int id,
        string batchNumber,
        int areaId,
        Area area,
        decimal receivedWeight,
        Leafbatchstatus status,
        Containertype containerType,
        Weathercondition weatherCondition,
        Leafcondition leafCondition)
    {
        var now = DateTime.UtcNow;

        return new Greenleafbatch
        {
            Id = id,
            BatchNumber = batchNumber,
            AreaId = areaId,
            Area = area,
            ContainerTypeId = containerType.Id,
            ContainerType = containerType,
            WeatherConditionId = weatherCondition.Id,
            WeatherCondition = weatherCondition,
            LeafConditionId = leafCondition.Id,
            LeafCondition = leafCondition,
            LeafBatchStatusId = status.Id,
            LeafBatchStatus = status,
            PluckingDateTime = now.AddHours(-3),
            CollectionDateTime = now.AddHours(-2),
            ArrivalDateTime = now.AddHours(-1),
            Weight = receivedWeight,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    private static async Task AddProcessingRecordAsync(
        UvaTeaDbContext context,
        Greenleafbatch batch,
        decimal processedWeight,
        DateTime completedAt)
    {
        context.Greenleafprocessingrecords.Add(
            new Greenleafprocessingrecord
            {
                GreenLeafBatchId = batch.Id,
                GreenLeafBatch = batch,
                ProcessedWeight = processedWeight,
                ProcessingNotes = null,
                CompletedAt = completedAt
            });

        await context.SaveChangesAsync();
    }

    private static async Task SeedBatchAsync(
        UvaTeaDbContext context,
        int batchId,
        string batchNumber,
        int areaId,
        decimal receivedWeight,
        decimal processedWeight,
        DateTime completedAt,
        string statusName = "PROCESSED")
    {
        var area = await context.Areas.SingleAsync(x => x.Id == areaId);
        var status = await context.Leafbatchstatuses
            .SingleAsync(x => x.Name == statusName);
        var containerType = await context.Containertypes.SingleAsync();
        var weatherCondition = await context.Weatherconditions.SingleAsync();
        var leafCondition = await context.Leafconditions.SingleAsync();

        var batch = CreateBatch(
            batchId,
            batchNumber,
            areaId,
            area,
            receivedWeight,
            status,
            containerType,
            weatherCondition,
            leafCondition);

        context.Greenleafbatches.Add(batch);
        await context.SaveChangesAsync();

        await AddProcessingRecordAsync(
            context,
            batch,
            processedWeight,
            completedAt);
    }

    [Fact]
    public async Task Handle_MultipleCompletedBatches_ReturnsCorrectTotalsAndYield()
    {
        var (context, handler) = await CreateSutAsync();

        var completedAt = new DateTime(
            2026, 10, 20, 10, 0, 0, DateTimeKind.Utc);

        await SeedBatchAsync(
            context, 1, "GLB-001", 1, 100m, 50m, completedAt);

        await SeedBatchAsync(
            context, 2, "GLB-002", 1, 300m, 90m,
            completedAt.AddHours(1));

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, null),
            CancellationToken.None);

        response.TotalReceivedWeight.Should().Be(400m);
        response.TotalProcessedWeight.Should().Be(140m);
        response.OverallYieldPercentage.Should().Be(35m);
        response.CompletedBatchCount.Should().Be(2);
        response.Batches.Should().HaveCount(2);

        response.Batches.Should().ContainSingle(
            x => x.BatchNumber == "GLB-001"
                 && x.YieldPercentage == 50m);

        response.Batches.Should().ContainSingle(
            x => x.BatchNumber == "GLB-002"
                 && x.YieldPercentage == 30m);
    }

    [Fact]
    public async Task Handle_UsesWeightedOverallYield_NotAverageOfBatchYields()
    {
        var (context, handler) = await CreateSutAsync();

        var completedAt = new DateTime(
            2026, 10, 20, 10, 0, 0, DateTimeKind.Utc);

        // Batch 1 yield = 50%; Batch 2 yield = 30%.
        // Average of yields = 40%, but weighted overall yield = 35%.
        await SeedBatchAsync(
            context, 1, "GLB-001", 1, 100m, 50m, completedAt);

        await SeedBatchAsync(
            context, 2, "GLB-002", 1, 300m, 90m,
            completedAt.AddHours(1));

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, null),
            CancellationToken.None);

        response.OverallYieldPercentage.Should().Be(35m);
        response.OverallYieldPercentage.Should().NotBe(40m);
    }

    [Fact]
    public async Task Handle_DateRange_ReturnsOnlyMatchingCompletionRecords()
    {
        var (context, handler) = await CreateSutAsync();

        await SeedBatchAsync(
            context, 1, "GLB-001", 1, 100m, 50m,
            new DateTime(2026, 10, 19, 23, 0, 0, DateTimeKind.Utc));

        await SeedBatchAsync(
            context, 2, "GLB-002", 1, 200m, 80m,
            new DateTime(2026, 10, 20, 12, 0, 0, DateTimeKind.Utc));

        await SeedBatchAsync(
            context, 3, "GLB-003", 1, 300m, 90m,
            new DateTime(2026, 10, 21, 0, 0, 0, DateTimeKind.Utc));

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(
                new DateTime(2026, 10, 20),
                new DateTime(2026, 10, 20),
                null),
            CancellationToken.None);

        response.CompletedBatchCount.Should().Be(1);
        response.Batches.Should().ContainSingle()
            .Which.BatchNumber.Should().Be("GLB-002");

        response.TotalReceivedWeight.Should().Be(200m);
        response.TotalProcessedWeight.Should().Be(80m);
    }

    [Fact]
    public async Task Handle_AreaFilter_ReturnsOnlyMatchingAreaBatches()
    {
        var (context, handler) = await CreateSutAsync();

        var completedAt = new DateTime(
            2026, 10, 20, 10, 0, 0, DateTimeKind.Utc);

        await SeedBatchAsync(
            context, 1, "GLB-001", 1, 100m, 50m, completedAt);

        await SeedBatchAsync(
            context, 2, "GLB-002", 2, 200m, 80m, completedAt);

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, 2),
            CancellationToken.None);

        response.CompletedBatchCount.Should().Be(1);
        response.Batches.Should().ContainSingle()
            .Which.BatchNumber.Should().Be("GLB-002");

        response.TotalReceivedWeight.Should().Be(200m);
        response.TotalProcessedWeight.Should().Be(80m);
        response.OverallYieldPercentage.Should().Be(40m);
    }

    [Fact]
    public async Task Handle_NoProcessingRecords_ReturnsZeroTotals()
    {
        var (_, handler) = await CreateSutAsync();

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, null),
            CancellationToken.None);

        response.TotalReceivedWeight.Should().Be(0m);
        response.TotalProcessedWeight.Should().Be(0m);
        response.OverallYieldPercentage.Should().Be(0m);
        response.CompletedBatchCount.Should().Be(0);
        response.Batches.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ZeroReceivedWeight_DoesNotDivideByZero()
    {
        var (context, handler) = await CreateSutAsync();

        await SeedBatchAsync(
            context,
            1,
            "GLB-001",
            1,
            0m,
            0m,
            new DateTime(2026, 10, 20, 10, 0, 0, DateTimeKind.Utc));

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, null),
            CancellationToken.None);

        response.CompletedBatchCount.Should().Be(1);
        response.Batches.Single().YieldPercentage.Should().Be(0m);
        response.OverallYieldPercentage.Should().Be(0m);
    }

    [Fact]
    public async Task Handle_ExcludesBatchesNotInProcessedStatus()
    {
        var (context, handler) = await CreateSutAsync();

        var completedAt = new DateTime(
            2026, 10, 20, 10, 0, 0, DateTimeKind.Utc);

        await SeedBatchAsync(
            context, 1, "GLB-001", 1, 100m, 50m, completedAt);

        await SeedBatchAsync(
            context, 2, "GLB-002", 1, 200m, 80m,
            completedAt, statusName: "PROCESSING");

        var response = await handler.Handle(
            new GetGreenLeafYieldAnalysisQuery(null, null, null),
            CancellationToken.None);

        response.CompletedBatchCount.Should().Be(1);
        response.Batches.Should().ContainSingle()
            .Which.BatchNumber.Should().Be("GLB-001");

        response.TotalReceivedWeight.Should().Be(100m);
        response.TotalProcessedWeight.Should().Be(50m);
    }
}
