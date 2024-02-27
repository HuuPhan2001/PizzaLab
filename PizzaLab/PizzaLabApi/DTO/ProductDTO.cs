using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaLabApi.Models;

public partial class ProductDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public double Weight { get; set; }

    public string Image { get; set; } = null!;

    public int CategoryId { get; set; }

    public int Ammount { get; set; }
    public virtual CategoryDTO Category { get; set; } = null!;


    public virtual ICollection<ReviewDTO> Reviews { get; } = new List<ReviewDTO>();
}
