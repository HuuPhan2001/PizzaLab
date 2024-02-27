using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PizzaLabApi.Models;

public partial class UserDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    [RegularExpression("(^0\\d{9}$)|(^$)", ErrorMessage = "Số điện thoại bạn nhập không hợp lệ")]// regex cho co so dien thoai thi check, khong co thi thoi
    public string? Phone { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? ContactName { get; set; }

    public int RoleId { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<OrderDTO> Orders { get; } = new List<OrderDTO>();

    public virtual ICollection<ReviewDTO> Reviews { get; } = new List<ReviewDTO>();

    public virtual RoleDTO Role { get; set; } = null!;
}
