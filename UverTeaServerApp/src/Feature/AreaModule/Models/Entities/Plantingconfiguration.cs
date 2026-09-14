using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

public partial class Plantingconfiguration
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Rowspacingfeet { get; set; }

    public decimal Plantspacingfeet { get; set; }

    public string? Description { get; set; }

    public bool Isactive { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
}