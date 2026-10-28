using System;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

public partial class Greenleafbatch
{
    public int Id { get; set; }

    public string BatchNumber { get; set; } = null!;

    // Foreign Keys
    public int AreaId { get; set; }
    public int ContainerTypeId { get; set; }
    public int WeatherConditionId { get; set; }
    public int LeafConditionId { get; set; }
    public int LeafBatchStatusId { get; set; }

    // Batch Details
    public DateTime PluckingDateTime { get; set; }

    public DateTime CollectionDateTime { get; set; }

    public DateTime ArrivalDateTime { get; set; }

    public decimal Weight { get; set; }

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public virtual Area Area { get; set; } = null!;

    public virtual Containertype ContainerType { get; set; } = null!;

    public virtual Weathercondition WeatherCondition { get; set; } = null!;

    public virtual Leafcondition LeafCondition { get; set; } = null!;

    public virtual Leafbatchstatus LeafBatchStatus { get; set; } = null!;

    public virtual Greenleafriskassessment? RiskAssessment { get; set; }

    public virtual User? RejectedByUser { get; set; }

    public string? RejectionReason { get; set; }

    public DateTime? RejectedAt { get; set; }

    public int? RejectedByUserId { get; set; }
}