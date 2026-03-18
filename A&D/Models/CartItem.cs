using System;
using System.Collections.Generic;

namespace A_D.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int MilkId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Cart { get; set; } = null!;

    public virtual Milk Milk { get; set; } = null!;
}
