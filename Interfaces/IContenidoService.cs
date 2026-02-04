using TurneroApi.DTOs.Contenido;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IContenidoService
{
  Task<PagedResult<ContenidoDto>> GetContenidosAsync(int page, int pageSize);
  Task<ContenidoDto?> GetContenidoAsync(uint id);
  Task<(List<Contenido> contenidos, string? errorMessage)> CreateContenidosAsync(List<IFormFile> archivos, List<bool> activos);
  Task<(Contenido? contenido, string? errorMessage)> UpdateContenidoAsync(uint id, ContenidoActualizarDto dto);
  Task<(bool deleted, string? errorMessage)> DeleteContenidoAsync(uint id);
}