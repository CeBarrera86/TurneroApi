using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IPuestoService
    {
        Task<Puesto?> GetPuestoAsync(uint id);
        Task<(Puesto? puesto, string? errorMessage)> CreatePuestoAsync(PuestoCrearDto puestoCrearDto);
        Task<(Puesto? puesto, string? errorMessage)> UpdatePuestoAsync(uint id, PuestoActualizarDto puestoActualizarDto);
        Task<(Puesto? puesto, string? errorMessage)> RegistrarLoginAsync(uint puestoId, uint usuarioId);
        Task<(Puesto? puesto, string? errorMessage)> RegistrarLogoutAsync(uint puestoId);
    }
}