using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs.Cliente;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using TurneroApi.Validation;

namespace TurneroApi.Services;

public class ClienteService : IClienteService
{
  private readonly TurneroDbContext _context;
  private readonly IMapper _mapper;
  private readonly IClienteRemotoService _clienteRemotoService;
  private readonly ILogger<ClienteService> _logger;

  private const ulong GUEST_CLIENT_ID = 1;

  public ClienteService(
      TurneroDbContext context,
      IMapper mapper,
      IClienteRemotoService clienteRemotoService,
      ILogger<ClienteService> logger)
  {
    _context = context;
    _mapper = mapper;
    _clienteRemotoService = clienteRemotoService;
    _logger = logger;
  }

  public async Task<Cliente?> ObtenerCliente(string dni)
  {
    return await _context.Clientes.FirstOrDefaultAsync(c => c.Dni == dni);
  }

  public async Task<Cliente?> GetClienteByIdAsync(ulong id)
  {
    return await _context.Clientes.FindAsync(id);
  }

  public async Task<IEnumerable<Cliente>> GetClientesAsync(int page, int pageSize)
  {
    return await _context.Clientes
        .OrderBy(c => c.Titular)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
  }

  public async Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto)
  {
    var dniError = await ClienteValidator.ValidateDniAsync(_context, clienteCrearDto.Dni);
    if (dniError != null) return (null, dniError);

    var cliente = _mapper.Map<Cliente>(clienteCrearDto);

    _context.Clientes.Add(cliente);
    try
    {
      await _context.SaveChangesAsync();
      return (cliente, null);
    }
    catch (DbUpdateException ex)
    {
      return (null, $"Error al crear el cliente: {ex.Message}");
    }
  }

  public async Task<ClienteDto?> ObtenerClientePorDni(string dni)
  {
    var clienteLocal = await ObtenerCliente(dni);
    if (clienteLocal != null)
      return _mapper.Map<ClienteDto>(clienteLocal);

    _logger.LogInformation("Cliente {Dni} no encontrado localmente. Buscando en base de datos remota...", dni);

    var clienteGeaPico = await _clienteRemotoService.ObtenerClienteGeaPico(dni);
    if (clienteGeaPico != null)
    {
      var newClienteDto = new ClienteCrearDto
      {
        Dni = clienteGeaPico.Dni,
        Titular = clienteGeaPico.Titular!
      };

      var (createdCliente, createError) = await CreateClienteAsync(newClienteDto);
      if (createdCliente != null)
        return _mapper.Map<ClienteDto>(createdCliente);

      _logger.LogError("Fallo al crear cliente local desde base remota para DNI {Dni}. Error: {Error}", dni, createError);
    }

    var guestClient = await _context.Clientes.FindAsync(GUEST_CLIENT_ID);
    return _mapper.Map<ClienteDto>(guestClient);
  }
}
