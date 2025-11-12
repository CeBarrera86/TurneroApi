using TurneroApi.DTOs.Puesto;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
  public interface IPuestoService
  {
    Task<Puesto?> GetPuestoAsync(int id);
    Task<(Puesto? puesto, string? errorMessage)> CreatePuestoAsync(PuestoCrearDto dto);
    Task<(Puesto? puesto, string? errorMessage)> UpdatePuestoAsync(int id, PuestoActualizarDto dto);
  }
}