using TurneroApi.DTOs;
using TurneroApi.Models;

namespace TurneroApi.Interfaces
{
    public interface IClienteService
    {
        Task<Cliente?> ObtenerCliente(string dni);
        Task<Cliente?> GetClienteByIdAsync(ulong id);
        Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto);
        Task<IEnumerable<Cliente>> GetClientesAsync(int page, int pageSize);

        Task<ClienteDto?> ObtenerClientePorDni(string dni);
    }
}