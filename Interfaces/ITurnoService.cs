using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface ITurnoService
    {
        Task<IEnumerable<Turno>> GetTurnosAsync();
        Task<Turno?> GetTurnoAsync(ulong id);
        Task<(Turno? turno, string? errorMessage)> CreateTurnoAsync(TurnoCrearDto turnoCrearDto);
        Task<(Turno? turno, string? errorMessage)> UpdateTurnoAsync(ulong id, TurnoActualizarDto turnoActualizarDto);
        Task<Turno?> GetTurnoActivoPorPuestoIdAsync(uint puestoId);
    }
}