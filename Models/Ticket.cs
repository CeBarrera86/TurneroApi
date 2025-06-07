using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Ticket
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public uint Numero { get; set; }
    public DateOnly FechaTicket { get; set; }
    public ulong Cliente { get; set; }
    public ulong Sector { get; set; }
    public ulong Estado { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual Cliente ClienteNavigation { get; set; } = null!;
    public virtual Estado EstadoNavigation { get; set; } = null!;
    public virtual Sector SectorNavigation { get; set; } = null!;
    public virtual ICollection<Turno> Turno { get; set; } = new List<Turno>();
}
