using System;
using System.Collections.Generic;
using UverTeaServerApp.src.Feature.FertilizerDistributionModule.Models.Entities;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

public partial class Fertilizer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int BrandId { get; set; }

    public int FertilizertypeId { get; set; }

    public decimal? Quantity { get; set; }

    public decimal? Unitprice { get; set; }

    public decimal? Rop { get; set; }

    public int FertilizerstatusId { get; set; }

    public DateOnly? Dointroduced { get; set; }

    public int UserId { get; set; }

    public virtual Fertilzerbrand Brand { get; set; } = null!;

    public virtual ICollection<Fertilizerdistribution> Fertilizerdistributions { get; set; } = new List<Fertilizerdistribution>();

    public virtual Fertilizerstatus Fertilizerstatus { get; set; } = null!;

    public virtual Fertilizertype Fertilizertype { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
