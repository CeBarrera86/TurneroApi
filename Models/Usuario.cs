using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Usuario
{
	public int Id { get; set; }
	public string Nombre { get; set; } = null!;
	public string Apellido { get; set; } = null!;
	public string Username { get; set; } = null!;
	public int RolId { get; set; }
	public bool Activo { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	public virtual Rol RolNavigation { get; set; } = null!;
	public virtual ICollection<Puesto> Puestos { get; set; } = new List<Puesto>();
	public virtual ICollection<Historial> Historiales { get; set; } = new List<Historial>();
}