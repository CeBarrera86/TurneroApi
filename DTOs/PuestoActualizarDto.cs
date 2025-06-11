using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class PuestoActualizarDto
    {
        [Range(0, uint.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo o cero.")]
        public uint UsuarioId { get; set; }

        [Range(0, uint.MaxValue, ErrorMessage = "El ID de mostrador debe ser un número positivo o cero.")]
        public uint MostradorId { get; set; }
    }
}