using System;
using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class HistorialCrearDto
    {
        [Required(ErrorMessage = "El ID del ticket es obligatorio.")]
        [Range(1, ulong.MaxValue, ErrorMessage = "El ID del ticket debe ser un número positivo.")]
        public ulong TicketId { get; set; }

        [Required(ErrorMessage = "El ID del estado es obligatorio.")]
        [Range(1, uint.MaxValue, ErrorMessage = "El ID del estado debe ser un número positivo.")]
        public uint EstadoId { get; set; }

        // Fecha se establecerá en el servicio como DateTime.Now

        public uint? PuestoId { get; set; }
        public ulong? TurnoId { get; set; }
        public uint? UsuarioId { get; set; }

        [MaxLength(255, ErrorMessage = "Los comentarios no pueden exceder los 255 caracteres.")]
        public string? Comentarios { get; set; }
    }
}