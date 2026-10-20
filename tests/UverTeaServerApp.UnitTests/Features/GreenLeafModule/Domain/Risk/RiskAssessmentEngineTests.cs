using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Risk;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Domain.Risk;

public class RiskAssessmentEngineTests
{
    [Theory]
    [InlineData(20, "LOW")]
    [InlineData(21, "MEDIUM")]
    [InlineData(40, "MEDIUM")]
    [InlineData(41, "HIGH")]
    [InlineData(60, "HIGH")]
    [InlineData(61, "CRITICAL")]
    public async Task AssessAsync_ShouldDetermineCorrectRiskLevel(
        int totalScore,
        string expectedRiskLevel)
    {
        var engine = CreateEngine(totalScore);

        var result = await engine.AssessAsync(
            new Greenleafbatch(),
            CancellationToken.None);

        result.RiskScore.Should().Be(totalScore);
        result.RiskLevel.Should().Be(expectedRiskLevel);
    }

    [Fact]
    public async Task AssessAsync_ShouldSumScoresFromAllRules()
    {
        var engine = new RiskAssessmentEngine(
        [
            CreateRule(10, "Rule A"),
            CreateRule(20, "Rule B"),
            CreateRule(5, "Rule C")
        ]);

        var result = await engine.AssessAsync(
            new Greenleafbatch(),
            CancellationToken.None);

        result.RiskScore.Should().Be(35);
        result.RiskLevel.Should().Be("MEDIUM");
        result.RuleResults.Should().HaveCount(3);
        result.RuleResults.Select(x => x.Score)
            .Should().Equal(10, 20, 5);
    }

    [Fact]
    public async Task AssessAsync_WhenNoRulesExist_ShouldReturnLowRisk()
    {
        var engine = new RiskAssessmentEngine(
            Enumerable.Empty<IRiskRule>());

        var result = await engine.AssessAsync(
            new Greenleafbatch(),
            CancellationToken.None);

        result.RiskScore.Should().Be(0);
        result.RiskLevel.Should().Be("LOW");
        result.RuleResults.Should().BeEmpty();
    }

    private static RiskAssessmentEngine CreateEngine(int score)
    {
        return new RiskAssessmentEngine(
        [
            CreateRule(score, "Test Rule")
        ]);
    }

    private static IRiskRule CreateRule(int score, string ruleName)
    {
        return new StubRiskRule(
            new RiskRuleResult(
                ruleName,
                score,
                $"{ruleName} test reason."));
    }

    private sealed class StubRiskRule(RiskRuleResult result) : IRiskRule
    {
        public RiskRuleResult Evaluate(Greenleafbatch batch) => result;
    }
}