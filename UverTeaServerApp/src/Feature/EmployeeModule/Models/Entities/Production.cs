using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Production
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public int ProductionorderId { get; set; }

    public virtual Productionorder Productionorder { get; set; } = null!;

    public virtual ICollection<Productionproduct> Productionproducts { get; set; } = new List<Productionproduct>();
}
