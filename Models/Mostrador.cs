using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Mostrador
{
    public ulong Id { get; set; }
    public uint Numero { get; set; }
    public string Ip { get; set; } = null!;
    public string? Alfa { get; set; }
    public string? Tipo { get; set; }
    public ulong Sector { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Puesto> Puesto { get; set; } = new List<Puesto>();
    public virtual Sector SectorNavigation { get; set; } = null!;
}
