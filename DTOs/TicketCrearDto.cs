using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class TicketCrearDto
    {
        [Required(ErrorMessage = "La letra del ticket es obligatoria.")]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "La letra debe tener entre 1 y 2 caracteres.")]
        [RegularExpression("^[A-Z]{1,2}$", ErrorMessage = "La letra debe contener solo caracteres alfabéticos en mayúscula (ej. C, R, UV).")]
        public string Letra { get; set; } = null!;

        [Required(ErrorMessage = "El ID de cliente es obligatorio.")]
        [Range(1, ulong.MaxValue, ErrorMessage = "El ID de cliente debe ser un número positivo.")]
        public ulong ClienteId { get; set; }

        [Required(ErrorMessage = "El ID del sector de origen es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID del sector de origen debe ser un número positivo.")]
        public uint SectorIdOrigen { get; set; }
    }
}