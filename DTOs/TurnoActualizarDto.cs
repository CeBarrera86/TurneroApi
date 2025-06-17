using System;
using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs
{
    public class TurnoActualizarDto
    {
        // Puedes actualizar el estado del turno
        [Range(1, uint.MaxValue, ErrorMessage = "El ID de estado debe ser un número positivo.")]
        public uint? EstadoId { get; set; }

        // Puedes marcar la fecha de fin (solo si el estado lo permite, lo manejará el servicio)
        // [DataType(DataType.DateTime)] // Esto es más para UI, la validación se hace en el servicio
        public DateTime? FechaFin { get; set; }
    }
}