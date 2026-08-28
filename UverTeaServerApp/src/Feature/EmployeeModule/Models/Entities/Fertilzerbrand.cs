using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Fertilzerbrand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Fertilizer> Fertilizers { get; set; } = new List<Fertilizer>();
}
