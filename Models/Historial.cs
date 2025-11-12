namespace TurneroApi.Models;

public partial class Historial
{
  public ulong Id { get; set; }
  public ulong TicketId { get; set; }
  public int EstadoId { get; set; }
  public DateTime Fecha { get; set; }
  public int? PuestoId { get; set; }
  public ulong? TurnoId { get; set; }
  public int? UsuarioId { get; set; }
  public string? Comentarios { get; set; }

  public virtual Ticket TicketNavigation { get; set; } = null!;
  public virtual Estado EstadoNavigation { get; set; } = null!;
  public virtual Puesto? PuestoNavigation { get; set; }
  public virtual Turno? TurnoNavigation { get; set; }
  public virtual Usuario? UsuarioNavigation { get; set; }
}