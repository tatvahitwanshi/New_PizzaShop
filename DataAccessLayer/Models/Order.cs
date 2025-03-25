using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Tablesid { get; set; }

    public int Customerid { get; set; }

    public int Paymentid { get; set; }

    public int Orderstatusid { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

    public decimal? Totalamount { get; set; }

    public short? Rating { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Orderapp> Orderapps { get; } = new List<Orderapp>();

    public virtual Orderstatus Orderstatus { get; set; } = null!;

    public virtual Payment Payment { get; set; } = null!;

    public virtual Table Tables { get; set; } = null!;
}
