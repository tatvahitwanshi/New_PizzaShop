using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class MapItemsModifiersgroup
{
    public int Mergrid { get; set; }

    public int Itemid { get; set; }

    public int Modifiersgroupid { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Modifiersgroup Modifiersgroup { get; set; } = null!;
}
