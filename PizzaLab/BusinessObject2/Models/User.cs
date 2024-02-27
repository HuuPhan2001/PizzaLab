using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Phone { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string ContactName { get; set; }

    public int RoleId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Review> Reviews { get; } = new List<Review>();

    public virtual Role Role { get; set; }
}
