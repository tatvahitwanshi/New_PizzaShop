using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Orderstatus
{
    public int Orderstatusid { get; set; }

    public BitArray Pending { get; set; } = null!;

    public BitArray Running { get; set; } = null!;

    public BitArray Inprogess { get; set; } = null!;

    public BitArray Completed { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
