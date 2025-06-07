using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Sector
{
    public ulong Id { get; set; }
    public string Letra { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual ICollection<Mostrador> Mostrador { get; set; } = new List<Mostrador>();
    public virtual ICollection<Tarea> Tarea { get; set; } = new List<Tarea>();
    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
}
