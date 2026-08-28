using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Privilage
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int ModuleId { get; set; }

    public int OperationId { get; set; }

    public string? Authority { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual Operation Operation { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
