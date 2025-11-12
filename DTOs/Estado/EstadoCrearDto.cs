using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Estado;

public class EstadoCrearDto
{
    [Required(ErrorMessage = "La letra es obligatoria.")]
    [StringLength(2, ErrorMessage = "La letra no puede exceder los 2 caracteres.")]
    [RegularExpression("^[A-Z]{1,2}$", ErrorMessage = "La letra debe contener solo letras mayúsculas.")]
    public string Letra { get; set; } = null!;

    [Required(ErrorMessage = "La descripción es obligatoria.")]
    [StringLength(20, ErrorMessage = "La descripción no puede exceder los 20 caracteres.")]
    public string Descripcion { get; set; } = null!;
}