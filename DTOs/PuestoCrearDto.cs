using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class PuestoCrearDto
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo.")]
        public uint UsuarioId { get; set; }

        [Required(ErrorMessage = "El ID de mostrador es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID de mostrador debe ser un número positivo.")]
        public uint MostradorId { get; set; }
    }
}