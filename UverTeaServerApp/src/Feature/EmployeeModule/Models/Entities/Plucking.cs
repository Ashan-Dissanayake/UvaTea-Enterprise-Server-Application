using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Plucking
{
    public int Id { get; set; }

    public int? AreaId { get; set; }

    public int? PluckerId { get; set; }

    public int PluckingseesionId { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public int LeaftypeId { get; set; }

    public int? Qty { get; set; }

    public int UserId { get; set; }

    public virtual Area? Area { get; set; }

    public virtual Leaftype Leaftype { get; set; } = null!;

    public virtual Employee? Plucker { get; set; }

    public virtual Pluckingseesion Pluckingseesion { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
