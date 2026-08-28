using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Productionproduct
{
    public int Id { get; set; }

    public int ProductionId { get; set; }

    public int ProductId { get; set; }

    public decimal? Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Production Production { get; set; } = null!;
}
