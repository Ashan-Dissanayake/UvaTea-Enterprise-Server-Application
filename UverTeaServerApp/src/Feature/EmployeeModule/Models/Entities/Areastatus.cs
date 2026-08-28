using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Areastatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
}
