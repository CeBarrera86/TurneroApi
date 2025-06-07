using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Puesto
{
    public ulong Id { get; set; }
    public ulong Mostrador { get; set; }
    public ulong User { get; set; }
    public DateTime? Login { get; set; }
    public DateTime? Logout { get; set; }
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual Mostrador MostradorNavigation { get; set; } = null!;
    public virtual ICollection<Turno> Turno { get; set; } = new List<Turno>();
    public virtual User UserNavigation { get; set; } = null!;
}
