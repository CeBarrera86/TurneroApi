using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class SectorActualizarDto
{
    [StringLength(3, ErrorMessage = "La letra no puede exceder los 3 caracteres.")]
    public string? Letra { get; set; }

    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string? Nombre { get; set; }

    [StringLength(120, ErrorMessage = "La descripción no puede exceder los 120 caracteres.")]
    public string? Descripcion { get; set; }

    public bool? Activo { get; set; }

    public uint? PadreId { get; set; }
}
