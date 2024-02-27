using System;
using System.Collections.Generic;

namespace PizzaLabApi.Models;

public partial class OrderDTO
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool Status { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? Freight { get; set; }
    public bool? IsCart { get; set; }

    public string ShipAddress { get; set; } = null!;

    public virtual ICollection<OrderDetailDTO> OrderDetails { get; } = new List<OrderDetailDTO>();

    public virtual UserDTO User { get; set; } = null!;
}
