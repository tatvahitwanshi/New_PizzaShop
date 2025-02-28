using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Invoice
{
    public int Invoiceid { get; set; }

    public string Invoicenumber { get; set; } = null!;

    public int Orderappid { get; set; }

    public virtual Orderapp Orderapp { get; set; } = null!;
}
