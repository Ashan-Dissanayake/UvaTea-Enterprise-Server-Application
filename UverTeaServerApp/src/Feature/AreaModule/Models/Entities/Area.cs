using System;
using System.Collections.Generic;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

public partial class Area
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal? Acres { get; set; }

    public DateOnly? DoAttached { get; set; }

    public int? PlantCount { get; set; }

    public DateOnly? DoProofing { get; set; }

    public int? SupervisorId { get; set; }

    public int AreaStatusId { get; set; }

    public int AreaCategoryId { get; set; }

    public int UserId { get; set; }

    public int? GrowthStageId { get; set; }

    public int? PlantingConfigurationId { get; set; }

    public virtual Areacategory AreaCategory { get; set; } = null!;

    public virtual ICollection<Areagrowthstagehistory> AreaGrowthStageHistories { get; set; } = new List<Areagrowthstagehistory>();

    public virtual Areastatus AreaStatus { get; set; } = null!;

    public virtual ICollection<Areastatushistory> AreaStatusHistories { get; set; } = new List<Areastatushistory>();

    public virtual Growthstage? GrowthStage { get; set; }

    public virtual Plantingconfiguration? PlantingConfiguration { get; set; }

    public virtual Employee? Supervisor { get; set; }

    public virtual User User { get; set; } = null!;
}