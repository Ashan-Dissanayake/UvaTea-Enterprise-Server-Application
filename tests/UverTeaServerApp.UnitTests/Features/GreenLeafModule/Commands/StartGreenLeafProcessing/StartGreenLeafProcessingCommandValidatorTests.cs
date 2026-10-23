using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.src.Feature.GreenLeafModule.Commands.StartGreenLeafProcessing;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Commands.StartGreenLeafProcessing;

public class StartGreenLeafProcessingCommandValidatorTests
{
    [Fact]
    public async Task Validate_ShouldFail_WhenBatchIdIsZero()
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new UvaTeaDbContext(options);

        var validator = new StartGreenLeafProcessingCommandValidator(context);

        var result = await validator.ValidateAsync(
            new StartGreenLeafProcessingCommand(0));

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_ShouldFail_WhenBatchDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new UvaTeaDbContext(options);

        var validator = new StartGreenLeafProcessingCommandValidator(context);

        var result = await validator.ValidateAsync(
            new StartGreenLeafProcessingCommand(999));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("not found"));
    }

    [Fact]
    public async Task Validate_ShouldPass_WhenBatchExists()
    {
        var options = new DbContextOptionsBuilder<UvaTeaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var context = new UvaTeaDbContext(options);

        context.Leafbatchstatuses.Add(new Leafbatchstatus
        {
            Id = 1,
            Name = "QUEUED"
        });

        await context.SaveChangesAsync();

        context.Greenleafbatches.Add(new Greenleafbatch
        {
            Id = 1,
            BatchNumber = "GL-VALIDATOR-001",
            LeafBatchStatusId = 1,
            PluckingDateTime = new DateTime(2026, 10, 22, 8, 0, 0),
            CollectionDateTime = new DateTime(2026, 10, 22, 9, 0, 0),
            ArrivalDateTime = new DateTime(2026, 10, 22, 10, 0, 0),
            Weight = 100m,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();

        var validator = new StartGreenLeafProcessingCommandValidator(context);

        var result = await validator.ValidateAsync(
            new StartGreenLeafProcessingCommand(1));

        result.IsValid.Should().BeTrue(
            string.Join("; ", result.Errors.Select(e => e.ErrorMessage)));
    }
}