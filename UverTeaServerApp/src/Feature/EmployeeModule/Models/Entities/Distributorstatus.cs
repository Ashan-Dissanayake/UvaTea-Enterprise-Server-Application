using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Distributorstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();
}
