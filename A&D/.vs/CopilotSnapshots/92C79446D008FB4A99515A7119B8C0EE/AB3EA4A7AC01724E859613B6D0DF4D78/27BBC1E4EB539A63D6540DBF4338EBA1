using System;
using System.Collections.Generic;

namespace A_D.Models;

public partial class Milk
{
    public int MilkId { get; set; }

    public string MilkName { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public DateTime ExpiryDate { get; set; }

    public string? ImagePath { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
