using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Usuario;

public class UsuarioCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(50)]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [StringLength(30)]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "El rol es obligatorio.")]
    public int RolId { get; set; }
}