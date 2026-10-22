using FluentAssertions;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;
using UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

namespace UverTeaServerApp.UnitTests.Features.GreenLeafModule.Domain;

public class ProcessingPriorityEngineTests
{
    private static readonly DateTime Now =
        new(2026, 10, 20, 12, 0, 0, DateTimeKind.Utc);

    private readonly ProcessingPriorityEngine _engine = new();

    [Fact]
    public void Rank_CriticalRiskBatch_ShouldRankBeforeLowRiskBatch()
    {
        var lowRisk = CreateBatch(
            id: 1,
            arrivalHoursAgo: 10,
            riskScore: 10,
            riskLevel: "LOW");

        var criticalRisk = CreateBatch(
            id: 2,
            arrivalHoursAgo: 1,
            riskScore: 65,
            riskLevel: "CRITICAL");

        var result = _engine.Rank(
            [lowRisk, criticalRisk],
            Now);

        result.Select(x => x.BatchId)
            .Should()
            .ContainInOrder(2, 1);

        result[0].PriorityRank.Should().Be(1);
        result[1].PriorityRank.Should().Be(2);
    }

    [Fact]
    public void Rank_SameRiskLevel_ShouldRankEarlierArrivalFirst()
    {
        var newerBatch = CreateBatch(
            id: 1,
            arrivalHoursAgo: 2,
            riskScore: 30,
            riskLevel: "HIGH");

        var olderBatch = CreateBatch(
            id: 2,
            arrivalHoursAgo: 5,
            riskScore: 35,
            riskLevel: "HIGH");

        var result = _engine.Rank(
            [newerBatch, olderBatch],
            Now);

        result.Select(x => x.BatchId)
            .Should()
            .ContainInOrder(2, 1);
    }

    [Fact]
    public void Rank_EmptyCollection_ShouldReturnEmptyResult()
    {
        var result = _engine.Rank(
            Enumerable.Empty<Greenleafbatch>(),
            Now);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Rank_BatchWithoutAssessment_ShouldThrow()
    {
        var batch = new Greenleafbatch
        {
            Id = 1,
            BatchNumber = "GL-001",
            ArrivalDateTime = Now.AddHours(-1),
            RiskAssessment = null!
        };

        var act = () => _engine.Rank([batch], Now);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*risk assessment*");
    }

    [Fact]
    public void Rank_FutureArrival_ShouldThrow()
    {
        var batch = CreateBatch(
            id: 1,
            arrivalHoursAgo: -1,
            riskScore: 20,
            riskLevel: "MEDIUM");

        var act = () => _engine.Rank([batch], Now);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("*future*");
    }

    private static Greenleafbatch CreateBatch(
        int id,
        int arrivalHoursAgo,
        int riskScore,
        string riskLevel)
    {
        return new Greenleafbatch
        {
            Id = id,
            BatchNumber = $"GL-{id:D3}",
            ArrivalDateTime = Now.AddHours(-arrivalHoursAgo),
            RiskAssessment = new Greenleafriskassessment
            {
                Id = id,
                GreenLeafBatchId = id,
                RiskScore = riskScore,
                RiskLevel = riskLevel,
                AssessedAt = Now
            }
        };
    }

    [Fact]
public void Rank_SameRiskAndArrivalTime_ShouldRankLowerBatchIdFirst()
{
    var higherId = CreateBatch(
        id: 2,
        arrivalHoursAgo: 3,
        riskScore: 30,
        riskLevel: "HIGH");

    var lowerId = CreateBatch(
        id: 1,
        arrivalHoursAgo: 3,
        riskScore: 30,
        riskLevel: "HIGH");

    var result = _engine.Rank([higherId, lowerId], Now);

    result.Select(x => x.BatchId)
        .Should()
        .ContainInOrder(1, 2);
}

[Fact]
public void Rank_AllRiskLevels_ShouldFollowRiskPriorityOrder()
{
    var batches = new[]
    {
        CreateBatch(1, 1, 10, "LOW"),
        CreateBatch(2, 1, 30, "MEDIUM"),
        CreateBatch(3, 1, 50, "HIGH"),
        CreateBatch(4, 1, 70, "CRITICAL")
    };

    var result = _engine.Rank(batches, Now);

    result.Select(x => x.RiskLevel)
        .Should()
        .ContainInOrder("CRITICAL", "HIGH", "MEDIUM", "LOW");

    result.Select(x => x.PriorityRank)
        .Should()
        .ContainInOrder(1, 2, 3, 4);
}

[Theory]
[InlineData("")]
[InlineData("   ")]
[InlineData("UNKNOWN")]
public void Rank_InvalidRiskLevel_ShouldThrow(string riskLevel)
{
    var batch = CreateBatch(1, 1, 10, riskLevel);

    var act = () => _engine.Rank([batch], Now);

    act.Should()
        .Throw<InvalidOperationException>()
        .WithMessage("*invalid risk level*");
}

[Fact]
public void Rank_NullCollection_ShouldThrow()
{
    var act = () => _engine.Rank(null!, Now);

    act.Should().Throw<ArgumentNullException>();
}

[Fact]
public void Rank_RiskLevelWithWhitespaceAndLowercase_ShouldNormalize()
{
    var batch = CreateBatch(1, 2, 65, "  critical  ");

    var result = _engine.Rank([batch], Now);

    result.Should().ContainSingle();
    result[0].RiskLevel.Should().Be("CRITICAL");
    result[0].PriorityRank.Should().Be(1);
}

[Fact]
public void Rank_ShouldCalculateWaitingTimeInPriorityReason()
{
    var batch = CreateBatch(1, 2, 30, "HIGH");

    var result = _engine.Rank([batch], Now);

    result[0].PriorityReason.Should()
        .Be("HIGH risk; waiting for 2.0 hours.");
}
}