using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.GreenLeafModule.Commands.ReceiveGreenLeafBatch;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Commands.ReceiveGreenLeafBatch;

public class ReceiveGreenLeafBatchCommandValidatorTests
{
    private static async Task<(UvaTeaDbContext Context,
        ReceiveGreenLeafBatchCommandValidator Validator)> CreateContextAsync(
        bool includeReferences = true,
        string areaStatus = "Active")
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new UvaTeaDbContext(options);

        if (includeReferences)
        {
            // Create one Areastatus instance and reuse it.
            var areaStatusEntity = new Areastatus
            {
                Id = 1,
                Name = areaStatus
            };

            context.Areastatuses.Add(areaStatusEntity);

            context.Areas.Add(new Area
            {
                Id = 1,
                AreaStatusId = areaStatusEntity.Id,
                AreaStatus = areaStatusEntity,
                AreaCategoryId = 1,
                UserId = 1
            });

            context.Containertypes.Add(new Containertype
            {
                Id = 1,
                Name = "Standard"
            });

            context.Weatherconditions.Add(new Weathercondition
            {
                Id = 1,
                Name = "Sunny"
            });

            context.Leafconditions.Add(new Leafcondition
            {
                Id = 1,
                Name = "FRESH"
            });

            await context.SaveChangesAsync();
        }

        return (context, new ReceiveGreenLeafBatchCommandValidator(context));
    }

    private static ReceiveGreenLeafBatchCommand ValidCommand() =>
        new(
            AreaId: 1,
            PluckingDateTime: new DateTime(2026, 10, 22, 8, 0, 0),
            CollectionDateTime: new DateTime(2026, 10, 22, 9, 0, 0),
            ArrivalDateTime: new DateTime(2026, 10, 22, 10, 0, 0),
            Weight: 100m,
            ContainerTypeId: 1,
            WeatherConditionId: 1,
            LeafConditionId: 1,
            Remarks: "Test batch"
        );

    [Fact]
    public async Task Validate_ShouldPass_WhenCommandIsValid()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var result = await validator.ValidateAsync(ValidCommand());

            result.IsValid.Should().BeTrue(
                string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenPluckingOccursAfterCollection()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var command = ValidCommand() with
            {
                PluckingDateTime = new DateTime(2026, 10, 22, 10, 0, 0),
                CollectionDateTime = new DateTime(2026, 10, 22, 9, 0, 0)
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Plucking time"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenCollectionOccursAfterArrival()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var command = ValidCommand() with
            {
                CollectionDateTime = new DateTime(2026, 10, 22, 11, 0, 0)
            };

            var result = await validator.ValidateAsync(command);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Plucking time"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenAreaIsInactive()
    {
        var (context, validator) = await CreateContextAsync(
            areaStatus: "Decommissioned");

        try
        {
            var result = await validator.ValidateAsync(ValidCommand());

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Area not found"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenAreaDoesNotExist()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var result = await validator.ValidateAsync(
                ValidCommand() with { AreaId = 999 });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Area not found"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenContainerTypeDoesNotExist()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var result = await validator.ValidateAsync(
                ValidCommand() with { ContainerTypeId = 999 });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Container Type"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenWeatherConditionDoesNotExist()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var result = await validator.ValidateAsync(
                ValidCommand() with { WeatherConditionId = 999 });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Weather Condition"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenLeafConditionDoesNotExist()
    {
        var (context, validator) = await CreateContextAsync();

        try
        {
            var result = await validator.ValidateAsync(
                ValidCommand() with { LeafConditionId = 999 });

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e =>
                e.ErrorMessage.Contains("Leaf Condition"));
        }
        finally
        {
            await context.DisposeAsync();
        }
    }
}
