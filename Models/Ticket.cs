using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Ticket
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public uint Numero { get; set; }
    public ulong ClienteId { get; set; }
    public DateTime Fecha { get; set; }
    public uint SectorIdOrigen { get; set; }
    public uint SectorIdActual { get; set; }
    public uint EstadoId { get; set; }
    public DateTime Actualizado { get; set; }

    public virtual Cliente ClienteNavigation { get; set; } = null!;
    public virtual Estado EstadoNavigation { get; set; } = null!;
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual Sector SectorIdActualNavigation { get; set; } = null!;
    public virtual Sector SectorIdOrigenNavigation { get; set; } = null!;
    public virtual ICollection<Turno> Turno { get; set; } = new List<Turno>();
}
