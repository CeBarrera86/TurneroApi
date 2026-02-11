using TurneroApi.DTOs.Cliente;
using TurneroApi.Models;

namespace TurneroApi.Interfaces;

public interface IClienteService
{
    Task<Cliente?> ObtenerCliente(string dni);
    Task<Cliente?> GetClienteByIdAsync(ulong id);
    Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto);
    Task<List<ClienteDto>> GetClientesAsync();
    Task<ClienteDto?> ObtenerClientePorDni(string dni);
}
