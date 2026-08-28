using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Order
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int DistributorId { get; set; }

    public DateOnly? Doorder { get; set; }

    public DateOnly? Doexpected { get; set; }

    public decimal? Expectedgrandtotal { get; set; }

    public int OrderstatusId { get; set; }

    public int UserId { get; set; }

    public virtual Distributor Distributor { get; set; } = null!;

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<Orderrproduct> Orderrproducts { get; set; } = new List<Orderrproduct>();

    public virtual Orderstatus Orderstatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
