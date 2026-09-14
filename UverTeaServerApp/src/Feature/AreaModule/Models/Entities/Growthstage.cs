using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

public partial class Growthstage
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Displayorder { get; set; }

    public bool Isactive { get; set; }

    public virtual ICollection<Areagrowthstagehistory> Areagrowthstagehistoryfrom_growthstages { get; set; } = new List<Areagrowthstagehistory>();

    public virtual ICollection<Areagrowthstagehistory> Areagrowthstagehistoryto_growthstages { get; set; } = new List<Areagrowthstagehistory>();

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();
}