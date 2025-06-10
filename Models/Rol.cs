using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Rol
{
    public uint Id { get; set; }
    public string Tipo { get; set; } = null!;
    public virtual ICollection<Usuario> Usuario { get; set; } = new List<Usuario>();
}
