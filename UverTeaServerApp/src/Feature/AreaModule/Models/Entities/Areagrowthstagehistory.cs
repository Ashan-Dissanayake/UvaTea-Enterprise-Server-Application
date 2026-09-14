using System;
using System.Collections.Generic;
using UverTeaServerApp.src.Feature.UserModule.Models.Entities;

namespace UverTeaServerApp.src.Feature.AreaModule.Models.Entities;

public partial class Areagrowthstagehistory
{
    public int Id { get; set; }

    public int Area_id { get; set; }

    public int From_growthstage_id { get; set; }

    public int To_growthstage_id { get; set; }

    public DateTime Changedat { get; set; }

    public int Changedby { get; set; }

    public string? Reason { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual User ChangedbyNavigation { get; set; } = null!;

    public virtual Growthstage From_growthstage { get; set; } = null!;

    public virtual Growthstage To_growthstage { get; set; } = null!;
}
