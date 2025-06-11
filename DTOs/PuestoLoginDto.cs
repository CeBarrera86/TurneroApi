using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class PuestoLoginDto
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio para el login.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo.")]
        public uint UsuarioId { get; set; }
    }
}