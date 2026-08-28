using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Distributor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Telephone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? Contactperson { get; set; }

    public string? Contactpersontp { get; set; }

    public string? Description { get; set; }

    public int DistributorstatusId { get; set; }

    public decimal? Creditlimit { get; set; }

    public int DistributortypeId { get; set; }

    public int UserId { get; set; }

    public virtual Distributorstatus Distributorstatus { get; set; } = null!;

    public virtual Distributortype Distributortype { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
