using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Sector
{
  public uint Id { get; set; }
  public uint? PadreId { get; set; }
  public string? Letra { get; set; }
  public string? Nombre { get; set; }
  public string? Descripcion { get; set; }
  public bool Activo { get; set; }

  public virtual ICollection<Sector> InversePadre { get; set; } = new List<Sector>();
  public virtual ICollection<Mostrador> Mostrador { get; set; } = new List<Mostrador>();
  public virtual Sector? Padre { get; set; }
  public virtual ICollection<Ticket> TicketsSectorIdActualNavigation { get; set; } = new List<Ticket>();
  public virtual ICollection<Ticket> TicketsSectorIdOrigenNavigation { get; set; } = new List<Ticket>();
}
