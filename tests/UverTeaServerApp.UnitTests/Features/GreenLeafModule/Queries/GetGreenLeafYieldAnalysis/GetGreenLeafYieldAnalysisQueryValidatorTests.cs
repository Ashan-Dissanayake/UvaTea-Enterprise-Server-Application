using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;
using Xunit;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Queries
    .GetGreenLeafYieldAnalysis;

public sealed class GetGreenLeafYieldAnalysisQueryValidatorTests
{
    private readonly GetGreenLeafYieldAnalysisQueryValidator _validator = new();

    [Fact]
    public void Validate_FromDateAfterToDate_ReturnsFailure()
    {
        var query = new GetGreenLeafYieldAnalysisQuery(
            new DateTime(2026, 10, 23),
            new DateTime(2026, 10, 20),
            null);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(
            error => error.PropertyName == nameof(query.FromDate));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidAreaId_ReturnsFailure(int areaId)
    {
        var query = new GetGreenLeafYieldAnalysisQuery(
            null,
            null,
            areaId);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(
            error => error.PropertyName == nameof(query.AreaId));
    }

    [Fact]
    public void Validate_ValidFilters_ReturnsSuccess()
    {
        var query = new GetGreenLeafYieldAnalysisQuery(
            new DateTime(2026, 10, 1),
            new DateTime(2026, 10, 23),
            2);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_NoOptionalFilters_ReturnsSuccess()
    {
        var query = new GetGreenLeafYieldAnalysisQuery(
            null,
            null,
            null);

        var result = _validator.Validate(query);

        result.IsValid.Should().BeTrue();
    }
}