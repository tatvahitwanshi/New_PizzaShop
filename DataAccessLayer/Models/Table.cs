using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Table
{
    public int Tablesid { get; set; }

    public string Tablename { get; set; } = null!;

    public string? Tablecapacity { get; set; }

    public bool? Isoccupied { get; set; }

    public bool? Isdeleted { get; set; }

    public int? Sectionid { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

    public virtual ICollection<MapOrderTable> MapOrderTables { get; } = new List<MapOrderTable>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Section? Section { get; set; }

    public virtual ICollection<WaitingTable> WaitingTables { get; } = new List<WaitingTable>();
}
