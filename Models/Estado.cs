namespace TurneroApi.Models;

public partial class Estado
{
  public int Id { get; set; }
  public string Letra { get; set; } = null!;
  public string Descripcion { get; set; } = null!;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
  public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
  public virtual ICollection<Historial> Historiales { get; set; } = new List<Historial>();
}