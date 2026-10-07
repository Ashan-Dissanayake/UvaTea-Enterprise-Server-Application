using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Models.Entities;

public partial class Leafbatchstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Greenleafbatch> Greenleafbatchs { get; set; } = new List<Greenleafbatch>();
}
