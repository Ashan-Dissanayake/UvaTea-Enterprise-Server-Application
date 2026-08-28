using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Orderstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
