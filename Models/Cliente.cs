using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Cliente
{
    public ulong Id { get; set; }
    public string Dni { get; set; } = null!;
    public string Titular { get; set; } = null!;
    public string? Celular { get; set; }
    public string? Email { get; set; }
    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
}
