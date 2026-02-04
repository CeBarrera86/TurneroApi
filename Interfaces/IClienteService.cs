using TurneroApi.DTOs.Cliente;
using TurneroApi.Models;
using TurneroApi.Utils;

namespace TurneroApi.Interfaces;

public interface IClienteService
{
    Task<Cliente?> ObtenerCliente(string dni);
    Task<Cliente?> GetClienteByIdAsync(ulong id);
    Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto);
    Task<PagedResult<ClienteDto>> GetClientesAsync(int page, int pageSize);
    Task<ClienteDto?> ObtenerClientePorDni(string dni);
}
