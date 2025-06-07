using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Tarea
{
    public ulong Id { get; set; }
    public ulong Sector { get; set; }
    public string Descripcion { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public virtual Sector SectorNavigation { get; set; } = null!;
}
