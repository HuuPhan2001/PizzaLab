using System;
using System.Collections.Generic;

namespace PizzaLabApi.Models;

public partial class OrderDetailDTO
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public virtual OrderDTO Order { get; set; } = null!;

    public virtual ProductDTO Product { get; set; } = null!;
}
