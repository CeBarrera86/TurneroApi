using TurneroApi.DTOs.Turno;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface ITurnoService
  {
    Task<List<TurnoDto>> GetTurnosAsync();
    Task<TurnoDto?> GetTurnoAsync(ulong id);
    Task<(Turno? turno, string? errorMessage)> CreateTurnoAsync(TurnoCrearDto turnoCrearDto, int puestoId);
    Task<(Turno? turno, string? errorMessage)> UpdateTurnoAsync(ulong id, TurnoActualizarDto turnoActualizarDto);
    Task<TurnoDto?> GetTurnoActivoPorPuestoIdAsync(int puestoId); // ← int en lugar de uint
  }
}