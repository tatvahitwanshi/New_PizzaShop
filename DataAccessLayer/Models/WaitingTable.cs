using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class WaitingTable
{
    public int Waitingid { get; set; }

    public int Customerid { get; set; }

    public bool? Isdeleted { get; set; }

    public int Tableid { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
