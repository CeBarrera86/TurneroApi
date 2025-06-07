using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Historial
{
    public ulong Id { get; set; }
    public ulong Turno { get; set; }
    public ulong Puesto { get; set; }
    public ulong Estado { get; set; }
    public ulong? DerPara { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual User? DerParaNavigation { get; set; }
    public virtual Estado EstadoNavigation { get; set; } = null!;
    public virtual Puesto PuestoNavigation { get; set; } = null!;
    public virtual Turno TurnoNavigation { get; set; } = null!;
}
