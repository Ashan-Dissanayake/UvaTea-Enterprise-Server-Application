using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Models.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Unitprice { get; set; }

    public decimal? Qtyonhand { get; set; }

    public virtual ICollection<Invoiceproduct> Invoiceproducts { get; set; } = new List<Invoiceproduct>();

    public virtual ICollection<Orderrproduct> Orderrproducts { get; set; } = new List<Orderrproduct>();

    public virtual ICollection<Productionproduct> Productionproducts { get; set; } = new List<Productionproduct>();
}
