using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class ClienteCrearDto
    {
        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(10, MinimumLength = 7, ErrorMessage = "El DNI debe tener entre 7 y 10 dígitos.")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El DNI solo puede contener números.")]
        public string Dni { get; set; } = null!;

        [Required(ErrorMessage = "El titular es obligatorio.")]
        [StringLength(50, ErrorMessage = "El titular no puede exceder los 50 caracteres.")]
        public string Titular { get; set; } = null!;
    }
}