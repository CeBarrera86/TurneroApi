namespace TurneroApi.Models;

public partial class Permiso
{
  public int Id { get; set; }
  public string Nombre { get; set; } = null!;
  public string? Descripcion { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();
}