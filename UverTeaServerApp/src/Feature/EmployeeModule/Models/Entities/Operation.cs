using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Operation
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ModuleId { get; set; }

    public virtual Module? Module { get; set; }

    public virtual ICollection<Privilage> Privilages { get; set; } = new List<Privilage>();
}
