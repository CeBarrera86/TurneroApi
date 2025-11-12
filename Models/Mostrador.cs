using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Mostrador
{
  public int Id { get; set; }
  public int Numero { get; set; }
  public string Ip { get; set; } = null!;
  public string? Tipo { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }

  public virtual ICollection<MostradorSector> MostradorSectores { get; set; } = new List<MostradorSector>();
  public virtual ICollection<Puesto> Puestos { get; set; } = new List<Puesto>();
}
