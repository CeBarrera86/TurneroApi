using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class RolCrearDto
{
    [Required(ErrorMessage = "El tipo de rol es obligatorio.")]
    [StringLength(50, ErrorMessage = "El tipo de rol no puede exceder los 50 caracteres.")]
    public string Tipo { get; set; } = null!;
}
