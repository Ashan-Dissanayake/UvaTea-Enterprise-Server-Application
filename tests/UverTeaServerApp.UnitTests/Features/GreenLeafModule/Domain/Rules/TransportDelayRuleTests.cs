using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Domain.Rules;

public class TransportDelayRuleTests
{
    private readonly TransportDelayRule _rule = new();

    [Theory]
    [InlineData(1, 0)]
    [InlineData(1.01, 10)]
    [InlineData(2, 10)]
    [InlineData(2.01, 20)]
    [InlineData(3, 20)]
    [InlineData(3.01, 30)]
    public void Evaluate_ShouldReturnExpectedScore(
        double transportHours,
        int expectedScore)
    {
        var collectionTime = new DateTime(
            2026, 10, 20, 8, 0, 0, DateTimeKind.Utc);

        var batch = new Greenleafbatch
        {
            CollectionDateTime = collectionTime,
            ArrivalDateTime = collectionTime.AddHours(transportHours)
        };

        var result = _rule.Evaluate(batch);

        result.Score.Should().Be(expectedScore);
        result.RuleName.Should().Be("Transport Delay");
        result.Reason.Should().NotBeNullOrWhiteSpace();
    }
}