namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

public partial class Greenleafriskassessment
{
    public int Id { get; set; }

    public int GreenLeafBatchId { get; set; }

    public int RiskScore { get; set; }

    public string RiskLevel { get; set; } = null!;

    public string? RiskReason { get; set; }

    public DateTime AssessedAt { get; set; }

    public virtual Greenleafbatch GreenLeafBatch { get; set; } = null!;
}