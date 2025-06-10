using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Estado
{
    public uint Id { get; set; }
    public string Letra { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
    public virtual ICollection<Turno> Turno { get; set; } = new List<Turno>();
}
