namespace TurneroApi.Models;

public partial class Puesto
{
  public int Id { get; set; }
  public int UsuarioId { get; set; }
  public int MostradorId { get; set; }
  public DateTime? Login { get; set; }
  public DateTime? Logout { get; set; }
  public bool Activo { get; set; }

  public virtual Usuario UsuarioNavigation { get; set; } = null!;
  public virtual Mostrador MostradorNavigation { get; set; } = null!;
  public virtual ICollection<Historial> Historiales { get; set; } = new List<Historial>();
  public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
