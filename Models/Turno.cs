using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Turno
{
    public ulong Id { get; set; }
    public ulong Ticket { get; set; }
    public ulong Puesto { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual Puesto PuestoNavigation { get; set; } = null!;
    public virtual Ticket TicketNavigation { get; set; } = null!;
}
