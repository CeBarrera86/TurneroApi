namespace TurneroApi.Models;

public partial class Ticket
{
  public ulong Id { get; set; }
  public string Letra { get; set; } = null!;
  public int Numero { get; set; }
  public ulong ClienteId { get; set; }
  public DateTime Fecha { get; set; }
  public int SectorIdOrigen { get; set; }
  public int? SectorIdActual { get; set; }
  public int EstadoId { get; set; }
  public DateTime? Actualizado { get; set; }

  public virtual Cliente ClienteNavigation { get; set; } = null!;
  public virtual Sector SectorIdOrigenNavigation { get; set; } = null!;
  public virtual Sector? SectorIdActualNavigation { get; set; }
  public virtual Estado EstadoNavigation { get; set; } = null!;
  public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
  public virtual ICollection<Historial> Historiales { get; set; } = new List<Historial>();
}