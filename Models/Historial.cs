using System;
using System.Collections.Generic;

namespace TurneroApi.Models;

public partial class Historial
{
    public ulong Id { get; set; }
    public ulong TicketId { get; set; }
    public uint EstadoId { get; set; }
    public DateTime Fecha { get; set; }
    public uint? PuestoId { get; set; }
    public ulong? TurnoId { get; set; }
    public uint? UsuarioId { get; set; }
    public string? Comentarios { get; set; }
    public virtual Ticket TicketNavigation { get; set; } = null!;
    public virtual Estado EstadoNavigation { get; set; } = null!;
    public virtual Puesto? PuestoNavigation { get; set; }
    public virtual Turno? TurnoNavigation { get; set; }
    public virtual Usuario? UsuarioNavigation { get; set; }
}
