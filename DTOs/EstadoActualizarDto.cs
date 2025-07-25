using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TurneroApi.DTOs;

public class EstadoActualizarDto
{
    [StringLength(1, ErrorMessage = "La letra debe ser un solo carácter.", MinimumLength = 1)]
    [RegularExpression(@"^[A-Za-z]$", ErrorMessage = "La letra debe ser un carácter alfabético simple (A-Z, a-z).")]
    public string? Letra { get; set; }

    [StringLength(50, ErrorMessage = "La descripción no puede exceder los 50 caracteres.")]
    public string? Descripcion { get; set; }
}