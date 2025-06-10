using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Mostrador
{
    public uint Id { get; set; }
    public uint Numero { get; set; }
    public string Ip { get; set; } = null!;
    public string? Tipo { get; set; }
    public uint SectorId { get; set; }
    public virtual ICollection<Puesto> Puesto { get; set; } = new List<Puesto>();
    public virtual Sector SectorNavigation { get; set; } = null!;
}
