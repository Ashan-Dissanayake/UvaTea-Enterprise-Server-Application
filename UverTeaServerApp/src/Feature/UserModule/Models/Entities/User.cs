using System;
using System.Collections.Generic;
using UverTeaServerApp.Shared.Entities;
using UverTeaServerApp.src.Feature.AreaModule.Models.Entities;
using UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;


namespace UverTeaServerApp.src.Feature.UserModule.Models.Entities;

public partial class User : IAuditableEntity, ISoftDeletable
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public DateOnly? Docreated { get; set; }

    public TimeOnly? Tocreated { get; set; }

    public int UserstatusId { get; set; }

    public int EmployeeId { get; set; }

    public string? Description { get; set; }

    public int RoleId { get; set; }

    public DateTime Createdat { get; set; }
    public DateTime? Updatedat { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual Employee Employee { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual Userstatus Userstatus { get; set; } = null!;
}
