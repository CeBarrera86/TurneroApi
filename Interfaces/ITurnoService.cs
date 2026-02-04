using TurneroApi.DTOs.Turno;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces
{
  public interface ITurnoService
  {
    Task<PagedResult<TurnoDto>> GetTurnosAsync(int page, int pageSize);
    Task<TurnoDto?> GetTurnoAsync(ulong id);
    Task<(Turno? turno, string? errorMessage)> CreateTurnoAsync(TurnoCrearDto turnoCrearDto);
    Task<(Turno? turno, string? errorMessage)> UpdateTurnoAsync(ulong id, TurnoActualizarDto turnoActualizarDto);
    Task<TurnoDto?> GetTurnoActivoPorPuestoIdAsync(int puestoId); // ← int en lugar de uint
  }
}