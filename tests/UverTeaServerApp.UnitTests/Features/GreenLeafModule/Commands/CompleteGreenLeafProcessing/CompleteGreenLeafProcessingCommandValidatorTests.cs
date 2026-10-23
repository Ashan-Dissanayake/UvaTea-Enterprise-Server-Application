
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Commands
    .CompleteGreenLeafProcessing;

public sealed class CompleteGreenLeafProcessingCommandValidatorTests
{
    private static async Task<(
        UvaTeaDbContext Context,
        CompleteGreenLeafProcessingCommandValidator Validator)>
        CreateSutAsync(bool includeBatch = true)
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UvaTeaDbContext(options);

        if (includeBatch)
        {
            var areaStatus = new Areastatus
            {
                Id = 1,
                Name = "ACTIVE"
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

            var status = new Leafbatchstatus
            {
                Id = 1,
                Name = "PROCESSING"
            };

            var batch = new Greenleafbatch
            {
                Id = 1,
                BatchNumber = "GLB-001",
                AreaId = 1,
                Area = area,
                ContainerTypeId = 1,
                ContainerType = containerType,
                WeatherConditionId = 1,
                WeatherCondition = weatherCondition,
                LeafConditionId = 1,
                LeafCondition = leafCondition,
                LeafBatchStatusId = 1,
                LeafBatchStatus = status,
                PluckingDateTime = DateTime.UtcNow.AddHours(-3),
                CollectionDateTime = DateTime.UtcNow.AddHours(-2),
                ArrivalDateTime = DateTime.UtcNow.AddHours(-1),
                Weight = 100m,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Areastatuses.Add(areaStatus);
            context.Areas.Add(area);
            context.Containertypes.Add(containerType);
            context.Weatherconditions.Add(weatherCondition);
            context.Leafconditions.Add(leafCondition);
            context.Leafbatchstatuses.Add(status);
            context.Greenleafbatches.Add(batch);

            await context.SaveChangesAsync();
        }

        var validator =
            new CompleteGreenLeafProcessingCommandValidator(context);

        return (context, validator);
    }

    [Fact]
    public async Task Validate_ValidRequest_Passes()
    {
        var (_, validator) = await CreateSutAsync();

        var command = new CompleteGreenLeafProcessingCommand(
            1,
            72.5m,
            "Normal processing");

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_InvalidBatchId_Fails(int batchId)
    {
        var (_, validator) = await CreateSutAsync();

        var command = new CompleteGreenLeafProcessingCommand(
            batchId,
            72.5m,
            null);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(
            error => error.PropertyName == nameof(command.GreenLeafBatchId));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Validate_NonPositiveProcessedWeight_Fails(
        decimal processedWeight)
    {
        var (_, validator) = await CreateSutAsync();

        var command = new CompleteGreenLeafProcessingCommand(
            1,
            processedWeight,
            null);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(
            error => error.PropertyName == nameof(command.ProcessedWeight));
    }

    [Fact]
    public async Task Validate_NotesExceedMaximumLength_Fails()
    {
        var (_, validator) = await CreateSutAsync();

        var command = new CompleteGreenLeafProcessingCommand(
            1,
            72.5m,
            new string('a', 1001));

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(
            error => error.PropertyName == nameof(command.ProcessingNotes));
    }

    [Fact]
    public async Task Validate_NonexistentBatch_Fails()
    {
        var (_, validator) = await CreateSutAsync(includeBatch: false);

        var command = new CompleteGreenLeafProcessingCommand(
            99,
            72.5m,
            null);

        var result = await validator.ValidateAsync(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(
            error => error.PropertyName == nameof(command.GreenLeafBatchId)
                && error.ErrorMessage == "Green leaf batch was not found.");
    }
}
