using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Productionorderstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Productionorder> Productionorders { get; set; } = new List<Productionorder>();
}
