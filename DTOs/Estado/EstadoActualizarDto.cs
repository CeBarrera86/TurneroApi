using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Estado;

public class EstadoActualizarDto
{
    [StringLength(2)]
    [RegularExpression("^[A-Z]{1,2}$", ErrorMessage = "La letra debe contener solo letras mayúsculas.")]
    public string? Letra { get; set; }

    [StringLength(20)]
    public string? Descripcion { get; set; }
}
