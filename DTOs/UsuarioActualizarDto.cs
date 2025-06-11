using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class UsuarioActualizarDto
{
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string? Nombre { get; set; }

    [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
    public string? Apellido { get; set; }

    [StringLength(30, ErrorMessage = "El nombre de usuario no puede exceder los 30 caracteres.")]
    public string? Username { get; set; }

    [Range(0, uint.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo o cero.")]
    public uint RolId { get; set; } 
}