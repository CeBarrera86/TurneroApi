using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Usuario
{
    public uint Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Username { get; set; } = null!;
    public uint RolId { get; set; }
    public virtual ICollection<Historial> Historial { get; set; } = new List<Historial>();
    public virtual ICollection<Puesto> Puesto { get; set; } = new List<Puesto>();
    public virtual Rol RolNavigation { get; set; } = null!;
}
