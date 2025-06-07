using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Rol
{
    public ulong Id { get; set; }
    public string Tipo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<User> User { get; set; } = new List<User>();
}
