namespace TurneroApi.Models;

public partial class Turno
{
  public ulong Id { get; set; }
  public int PuestoId { get; set; }
  public ulong TicketId { get; set; }
  public DateTime FechaInicio { get; set; }
  public DateTime? FechaFin { get; set; }
  public int EstadoId { get; set; }

  public virtual Puesto PuestoNavigation { get; set; } = null!;
  public virtual Ticket TicketNavigation { get; set; } = null!;
  public virtual Estado EstadoNavigation { get; set; } = null!;
  public virtual ICollection<Historial> Historiales { get; set; } = new List<Historial>();
}