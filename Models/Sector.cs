using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Sector
{
  public int Id { get; set; }
  public int? PadreId { get; set; }
  public string? Letra { get; set; }
  public string? Nombre { get; set; }
  public string? Descripcion { get; set; }
  public bool Activo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual Sector? Padre { get; set; }
  public virtual ICollection<Sector> InversePadre { get; set; } = new List<Sector>();

  public virtual ICollection<MostradorSector> MostradorSectores { get; set; } = new List<MostradorSector>();
  public virtual ICollection<Ticket> TicketsSectorIdOrigenNavigation { get; set; } = new List<Ticket>();
  public virtual ICollection<Ticket> TicketsSectorIdActualNavigation { get; set; } = new List<Ticket>();
}