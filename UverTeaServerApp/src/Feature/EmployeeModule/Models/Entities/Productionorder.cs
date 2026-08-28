using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Productionorder
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public int? AreaId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Humidity { get; set; }

    public string? Description { get; set; }

    public int ProductionorderstatusId { get; set; }

    public int UserId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual Productionorderstatus Productionorderstatus { get; set; } = null!;

    public virtual ICollection<Production> Productions { get; set; } = new List<Production>();

    public virtual User User { get; set; } = null!;
}
