using System;
using System.Collections.Generic;

namespace PizzaLabApi.Models;

public partial class RoleDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<UserDTO> Users { get; } = new List<UserDTO>();
}
