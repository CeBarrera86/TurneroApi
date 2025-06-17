using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class TurnoCrearDto
    {
        [Required(ErrorMessage = "El ID del puesto es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID del puesto debe ser un número positivo.")]
        public uint PuestoId { get; set; }

        [Required(ErrorMessage = "El ID del ticket es obligatorio.")]
        [Range(1, ulong.MaxValue, ErrorMessage = "El ID del ticket debe ser un número positivo.")]
        public ulong TicketId { get; set; }
    }
}