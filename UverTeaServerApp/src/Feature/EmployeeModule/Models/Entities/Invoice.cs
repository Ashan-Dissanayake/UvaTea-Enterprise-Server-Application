using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Invoice
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? Number { get; set; }

    public DateOnly? Date { get; set; }

    public decimal? Grandtotal { get; set; }

    public int InvoicestatusId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Invoiceproduct> Invoiceproducts { get; set; } = new List<Invoiceproduct>();

    public virtual Invoicestatus Invoicestatus { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
