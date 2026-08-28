using System;
using System.Collections.Generic;
using UverTeaServerApp.Shared.Entities;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Employee : IAuditableEntity, ISoftDeletable
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; } = false;

    public string? Number { get; set; }

    public string? Fullname { get; set; }

    public string? Callingname { get; set; }

    public int GenderId { get; set; }

    public DateOnly? Dobirth { get; set; }

    public string? Nic { get; set; }

    public string? Address { get; set; }

    public string? Mobile { get; set; }

    public string? Land { get; set; }

    public DateOnly? Doassignment { get; set; }

    public int DesignationId { get; set; }

    public int EmployeestatusId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual Designation Designation { get; set; } = null!;

    public virtual Employeestatus Employeestatus { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<Plucking> Pluckings { get; set; } = new List<Plucking>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public DateTime Createdat { get; set; }
    public DateTime? Updatedat { get; set; }
}
