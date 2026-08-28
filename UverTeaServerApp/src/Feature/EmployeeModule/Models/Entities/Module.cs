using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Module
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();

    public virtual ICollection<Privilage> Privilages { get; set; } = new List<Privilage>();
}
