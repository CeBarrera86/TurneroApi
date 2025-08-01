using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class TicketCrearDto
    {
        [Required(ErrorMessage = "El ID de cliente es obligatorio.")]
        [Range(1, ulong.MaxValue, ErrorMessage = "El ID de cliente debe ser un número positivo.")]
        public ulong ClienteId { get; set; }

        [Required(ErrorMessage = "El ID del sector de origen es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID del sector de origen debe ser un número positivo.")]
        public uint SectorIdOrigen { get; set; }
    }
}