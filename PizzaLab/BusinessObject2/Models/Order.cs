using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool Status { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? Freight { get; set; }

    public string ShipAddress { get; set; }

    public bool? IsCart { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual User User { get; set; }
}
