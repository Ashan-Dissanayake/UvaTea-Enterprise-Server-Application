using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk.Rules;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Domain.Rules;

public class LeafConditionRuleTests
{
    private readonly LeafConditionRule _rule = new();

    [Theory]
    [InlineData("FRESH", 0)]
    [InlineData("NORMAL", 5)]
    [InlineData("WILTED", 20)]
    [InlineData("DAMAGED", 30)]
    [InlineData("fresh", 0)]
    [InlineData("  wilted  ", 20)]
    [InlineData("UNKNOWN", 0)]
    public void Evaluate_ShouldReturnExpectedScore(
        string conditionName,
        int expectedScore)
    {
        var batch = new Greenleafbatch
        {
            LeafCondition = new Leafcondition
            {
                Name = conditionName
            }
        };

        var result = _rule.Evaluate(batch);

        result.Score.Should().Be(expectedScore);
        result.RuleName.Should().Be("Leaf Condition");
        result.Reason.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Evaluate_WhenConditionNameIsNull_ShouldReturnZeroScore()
    {
        var batch = new Greenleafbatch
        {
            LeafCondition = new Leafcondition
            {
                Name = null
            }
        };

        var result = _rule.Evaluate(batch);

        result.Score.Should().Be(0);
        result.Reason.Should().Contain("unknown");
    }
}