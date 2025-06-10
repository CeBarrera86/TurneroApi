using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Cliente
{
    public ulong Id { get; set; }
    public string Dni { get; set; } = null!;
    public string Titular { get; set; } = null!;
    public virtual ICollection<Ticket> Ticket { get; set; } = new List<Ticket>();
}
