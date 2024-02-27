using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public double Weight { get; set; }

    public string Image { get; set; }

    public int? Ammount { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();
}
