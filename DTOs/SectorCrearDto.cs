using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs;

public class SectorCrearDto : IValidatableObject
{
    [StringLength(3, ErrorMessage = "La letra no puede exceder los 3 caracteres.")]
    public string? Letra { get; set; }

    [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
    public string? Nombre { get; set; }

    [StringLength(120, ErrorMessage = "La descripción no puede exceder los 120 caracteres.")]
    public string? Descripcion { get; set; }

    public bool Activo { get; set; } = true;

    public uint? PadreId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PadreId == null)
        {
            if (string.IsNullOrWhiteSpace(Letra))
                yield return new ValidationResult("La letra es obligatoria para sectores raíz.", new[] { nameof(Letra) });

            if (string.IsNullOrWhiteSpace(Nombre))
                yield return new ValidationResult("El nombre es obligatorio para sectores raíz.", new[] { nameof(Nombre) });
        }
    }
}
