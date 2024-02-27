using System;
using System.Collections.Generic;

namespace PizzaLabApi.Models;

public partial class CategoryDTO
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProductDTO> Products { get; } = new List<ProductDTO>();
}
