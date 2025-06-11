using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class SectorCrearDto
{
    [Required(ErrorMessage = "La letra del sector es obligatoria.")]
    [StringLength(3, ErrorMessage = "La letra no puede exceder los 3 caracteres.")]
    public string Letra { get; set; } = null!;

    [Required(ErrorMessage = "El nombre del sector es obligatorio.")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string Nombre { get; set; } = null!;

    [StringLength(120, ErrorMessage = "La descripción no puede exceder los 120 caracteres.")]
    public string? Descripcion { get; set; }

    public uint? PadreId { get; set; }
}
