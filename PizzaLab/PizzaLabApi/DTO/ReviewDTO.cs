using System;
using System.Collections.Generic;

namespace PizzaLabApi.Models;

public partial class ReviewDTO
{
    public int Id { get; set; }

    public string Comment { get; set; } = null!;

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateTime DateModified { get; set; }

    public virtual ProductDTO Product { get; set; } = null!;

    public virtual UserDTO User { get; set; } = null!;
}
