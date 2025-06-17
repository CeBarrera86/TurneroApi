using System;

namespace TurneroApi.DTOs
{
    public class HistorialDto
    {
        public ulong Id { get; set; }
        public ulong TicketId { get; set; }
        public uint EstadoId { get; set; }
        public DateTime Fecha { get; set; }
        public uint? PuestoId { get; set; }
        public ulong? TurnoId { get; set; }
        public uint? UsuarioId { get; set; }
        public string? Comentarios { get; set; }

        // DTOs para las navegaciones para enriquecer la respuesta
        public TicketDto TicketNavigation { get; set; } = null!; // Podría ser un TicketSimpleDto si no necesitas todos los detalles
        public EstadoDto EstadoNavigation { get; set; } = null!;
        public PuestoDto? PuestoNavigation { get; set; }
        public TurnoDto? TurnoNavigation { get; set; } // Podría ser un TurnoSimpleDto
        public UsuarioDto? UsuarioNavigation { get; set; } // Si tienes UsuarioDto
    }
}