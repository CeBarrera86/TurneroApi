using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class User
{
    public ulong Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public ulong Role { get; set; }
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime? EmailVerifiedAt { get; set; }
    public string Password { get; set; } = null!;
    public string? RememberToken { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual ICollection<Puesto> Puesto { get; set; } = new List<Puesto>();
    public virtual Rol RoleNavigation { get; set; } = null!;
}
