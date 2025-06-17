using System;
using TurneroApi.Models; // Assuming Estado, Puesto, Ticket are in Models

namespace TurneroApi.DTOs
{
    public class TurnoDto
    {
        public ulong Id { get; set; }
        public uint PuestoId { get; set; }
        public ulong TicketId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public uint EstadoId { get; set; }

        // Incluir DTOs para las navegaciones si necesitas sus detalles
        public EstadoDto EstadoNavigation { get; set; } = null!;
        public PuestoDto PuestoNavigation { get; set; } = null!;
        public TicketDto TicketNavigation { get; set; } = null!;
        // No incluir Historial aquí a menos que sea específicamente necesario para una vista de turno
    }
}