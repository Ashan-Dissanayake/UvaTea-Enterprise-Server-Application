using System;
using System.Collections.Generic;
using UverTeaServerApp.src.Feature.ProductionOrderModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.ProductionModule.Models.Entities;

public partial class Production
{
    public int Id { get; set; }

    public DateOnly? Date { get; set; }

    public TimeOnly? Time { get; set; }

    public int ProductionorderId { get; set; }

    public virtual Productionorder Productionorder { get; set; } = null!;

    public virtual ICollection<Productionproduct> Productionproducts { get; set; } = new List<Productionproduct>();
}
