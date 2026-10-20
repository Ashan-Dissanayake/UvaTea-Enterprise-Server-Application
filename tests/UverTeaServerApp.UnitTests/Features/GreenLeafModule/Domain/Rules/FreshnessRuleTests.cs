using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Domain.Rules;

public class FreshnessRuleTests
{
    private readonly FreshnessRule _rule = new();

    [Theory]
    [InlineData(2, 0)]
    [InlineData(2.01, 10)]
    [InlineData(4, 10)]
    [InlineData(4.01, 20)]
    [InlineData(6, 20)]
    [InlineData(6.01, 30)]
    public void Evaluate_ShouldReturnExpectedScore(
        double elapsedHours,
        int expectedScore)
    {
        var pluckingTime = new DateTime(
            2026, 10, 20, 6, 0, 0, DateTimeKind.Utc);

        var batch = new Greenleafbatch
        {
            PluckingDateTime = pluckingTime,
            ArrivalDateTime = pluckingTime.AddHours(elapsedHours)
        };

        var result = _rule.Evaluate(batch);

        result.Score.Should().Be(expectedScore);
        result.RuleName.Should().Be("Freshness");
        result.Reason.Should().NotBeNullOrWhiteSpace();
    }
}