using System;
using System.ComponentModel.DataAnnotations;

namespace TurneroApi.DTOs.Turno;

public class TurnoActualizarDto
{
    public DateTime? FechaFin { get; set; }
    public int? EstadoId { get; set; }
}