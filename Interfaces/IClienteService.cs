using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IClienteService
    {
        // Obtener un cliente por su DNI (uso principal)
        Task<Cliente?> GetClienteByDniAsync(string dni);

        // Obtener un cliente por su ID (para uso interno o casos específicos)
        Task<Cliente?> GetClienteByIdAsync(ulong id);

        // Crear un nuevo cliente en la base de datos local
        Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto);
        
        // (Opcional) Obtener todos los clientes (con paginación, si la lista puede ser grande)
        Task<IEnumerable<Cliente>> GetClientesAsync(int page, int pageSize);
    }
}