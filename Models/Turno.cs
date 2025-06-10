using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Turno
{
    public ulong Id { get; set; }
    public uint PuestoId { get; set; }
    public ulong TicketId { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public uint EstadoId { get; set; }

    public virtual Estado EstadoNavigation { get; set; } = null!;
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual Puesto PuestoNavigation { get; set; } = null!;
    public virtual Ticket TicketNavigation { get; set; } = null!;
}
