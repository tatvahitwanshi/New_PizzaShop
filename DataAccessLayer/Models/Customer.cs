using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Customer
{
    public int Customerid { get; set; }

    public string Customername { get; set; } = null!;

    public string? Customeremail { get; set; }

    public string? PhoneNumber { get; set; }

    public string? CustomerDate { get; set; }

    public string Totalorder { get; set; } = null!;

    public bool? Iswaiting { get; set; }

    public int? NumberOfPerson { get; set; }

    public int? FoodReview { get; set; }

    public int? ServiceReview { get; set; }

    public int? AmbienceReview { get; set; }

    public string? Comment { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

    public virtual ICollection<MergeTable> MergeTables { get; } = new List<MergeTable>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<WaitingTable> WaitingTables { get; } = new List<WaitingTable>();
}
