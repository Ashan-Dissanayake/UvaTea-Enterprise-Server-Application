namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

public class Greenleafprocessingrecord
{
    public int Id { get; set; }

    public int GreenLeafBatchId { get; set; }

    public decimal ProcessedWeight { get; set; }

    public string? ProcessingNotes { get; set; }

    public DateTime CompletedAt { get; set; }

    public virtual Greenleafbatch GreenLeafBatch { get; set; } = null!;
}