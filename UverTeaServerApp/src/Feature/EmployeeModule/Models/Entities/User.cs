using System;
using System.Collections.Generic;
using UverTeaServerApp.Shared.Entities;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

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

    public virtual ICollection<Distributor> Distributors { get; set; } = new List<Distributor>();

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<Fertilizerdistribution> Fertilizerdistributions { get; set; } = new List<Fertilizerdistribution>();

    public virtual ICollection<Fertilizer> Fertilizers { get; set; } = new List<Fertilizer>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Plucking> Pluckings { get; set; } = new List<Plucking>();

    public virtual ICollection<Productionorder> Productionorders { get; set; } = new List<Productionorder>();

    public virtual Role Role { get; set; } = null!;

    public virtual Userstatus Userstatus { get; set; } = null!;
}
