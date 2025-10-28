using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Puesto
{
    public uint Id { get; set; }
    public uint UsuarioId { get; set; }
    public uint MostradorId { get; set; }
    public DateTime? Login { get; set; }
    public DateTime? Logout { get; set; }
    public bool? Activo { get; set; }

    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual ICollection<Turno> Turno { get; set; } = new List<Turno>();
    public virtual Mostrador MostradorNavigation { get; set; } = null!;
    public virtual Usuario UsuarioNavigation { get; set; } = null!;
}
