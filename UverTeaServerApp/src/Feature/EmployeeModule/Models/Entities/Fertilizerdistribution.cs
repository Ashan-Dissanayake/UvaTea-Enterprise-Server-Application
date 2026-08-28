using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Fertilizerdistribution
{
    public int Id { get; set; }

    public int? AreaId { get; set; }

    public int? FertilizerId { get; set; }

    public decimal? Quantity { get; set; }

    public DateOnly? Date { get; set; }

    public int FerdistributionstateId { get; set; }

    public int UserId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual Ferdistributionstate Ferdistributionstate { get; set; } = null!;

    public virtual Fertilizer? Fertilizer { get; set; }

    public virtual User User { get; set; } = null!;
}
