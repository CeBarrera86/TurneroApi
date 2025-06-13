using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class TicketActualizarDto
    {
        [Range(0, uint.MaxValue, ErrorMessage = "El ID del sector actual debe ser un número positivo o cero.")]
        public uint? SectorIdActual { get; set; }

        [Range(0, uint.MaxValue, ErrorMessage = "El ID de estado debe ser un número positivo o cero.")]
        public uint? EstadoId { get; set; }
    }
}