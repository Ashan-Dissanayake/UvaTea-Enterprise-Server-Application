using System;
using System.Collections.Generic;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;
using UverTeaServerApp.src.Feature.FertilizerDistributionModule.Models.Entities;
using UverTeaServerApp.src.Feature.PluckingModule.Models.Entities;
using UverTeaServerApp.src.Feature.ProductionOrderModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

public partial class Area
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal? Acres { get; set; }

    public DateOnly? Doattached { get; set; }

    public int? Plantcount { get; set; }

    public DateOnly? Doproofing { get; set; }

    public int? SupervisorId { get; set; }

    public int AreastatusId { get; set; }

    public int AreacategoryId { get; set; }

    public int UserId { get; set; }

    public virtual Areacategory Areacategory { get; set; } = null!;

    public virtual Areastatus Areastatus { get; set; } = null!;

    public virtual ICollection<Fertilizerdistribution> Fertilizerdistributions { get; set; } = new List<Fertilizerdistribution>();

    public virtual ICollection<Plucking> Pluckings { get; set; } = new List<Plucking>();

    public virtual ICollection<Productionorder> Productionorders { get; set; } = new List<Productionorder>();

    public virtual Employee? Supervisor { get; set; }

    public virtual User User { get; set; } = null!;
}
