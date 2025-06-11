using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class UsuarioCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres.")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30, ErrorMessage = "El nombre de usuario no puede exceder los 30 caracteres.")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "El ID del rol es obligatorio.")]
    [Range(1, uint.MaxValue, ErrorMessage = "El ID del rol debe ser un número positivo.")]
    public uint RolId { get; set; }
}