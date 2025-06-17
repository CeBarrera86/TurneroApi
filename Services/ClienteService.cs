// TurneroApi/Services/ClienteService.cs
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;
using Microsoft.Extensions.Logging;

namespace TurneroApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClienteRemotoService _clienteRemotoService; // Inject the remote service
        private readonly ILogger<ClienteService> _logger;

        // You might define the Guest Client somewhere centrally, like configuration or a static property
        private const ulong GUEST_CLIENT_ID = 1;
        private const string GUEST_CLIENT_DNI = "00000000"; // Or any other default DNI
        private const string GUEST_CLIENT_TITULAR = "Invitado";


        public ClienteService(TurneroDbContext context, IMapper mapper,
                              IClienteRemotoService clienteRemotoService, // Inject it
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
            // Existing pagination logic, useful for admin panel
            return await _context.Clientes
                                 .OrderBy(c => c.Titular)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto)
        {
            // Existing unique DNI validation
            var existingCliente = await _context.Clientes.AnyAsync(c => c.Dni == clienteCrearDto.Dni);
            if (existingCliente)
            {
                _logger.LogWarning("Intento de crear cliente con DNI existente: {Dni}", clienteCrearDto.Dni);
                return (null, $"Ya existe un cliente con el DNI '{clienteCrearDto.Dni}'.");
            }

            var cliente = _mapper.Map<Cliente>(clienteCrearDto);

            _context.Clientes.Add(cliente);
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cliente {Dni} creado exitosamente en la DB local. ID: {Id}", cliente.Dni, cliente.Id);
                return (cliente, null);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error al crear el cliente {Dni} en la DB local.", clienteCrearDto.Dni);
                return (null, $"Error al crear el cliente: {ex.Message}");
            }
        }

        public async Task<ClienteDto?> ObtenerClientePorDni(string dni)
        {
            _logger.LogInformation("Iniciando búsqueda de cliente por DNI: {Dni}", dni);

            // 1. Buscar en la base de datos local
            var clienteLocal = await ObtenerCliente(dni);
            if (clienteLocal != null)
            {
                _logger.LogInformation("Cliente {Dni} encontrado localmente. ID: {Id}", dni, clienteLocal.Id);
                return _mapper.Map<ClienteDto>(clienteLocal);
            }

            _logger.LogInformation("Cliente {Dni} no encontrado localmente. Buscando en base de datos remota...", dni);

            // 2. Si no se encuentra localmente, buscar en la base GeaPico
            var clienteGeaPico = await _clienteRemotoService.ObtenerClienteGeaPico(dni);
            if (clienteGeaPico != null)
            {
                _logger.LogInformation("Cliente {Dni} encontrado en base de datos remota. Creando copia local...", dni);

                // 3. Si se encuentra remotamente, crear una nueva tupla en la base de datos local
                var newClienteDto = new ClienteCrearDto
                {
                    Dni = clienteGeaPico.Dni,
                    Titular = clienteGeaPico.Titular
                };

                var (createdCliente, createError) = await CreateClienteAsync(newClienteDto);
                if (createdCliente != null)
                {
                    _logger.LogInformation("Cliente {Dni} creado localmente desde base remota. ID: {Id}", dni, createdCliente.Id);
                    return _mapper.Map<ClienteDto>(createdCliente);
                }
                else
                {
                    _logger.LogError("Fallo al crear cliente local desde base remota para DNI {Dni}. Error: {Error}", dni, createError);
                    // Si falla la creación local, podemos retornar al invitado o un error, según la política.
                    // Por simplicidad, retornamos al invitado.
                }
            }

            _logger.LogWarning("Cliente {Dni} no encontrado en base de datos local ni remota. Retornando cliente 'Invitado'.", dni);

            // 4. Si no se encuentra en ningún lado, devolver el cliente "Invitado"
            // Asegúrate de que el Cliente 'Invitado' con ID 1 exista en tu base de datos local
            // antes de iniciar la aplicación o créalo si no existe.
            var guestClient = await _context.Clientes.FindAsync(GUEST_CLIENT_ID);
            if (guestClient == null)
            {
                _logger.LogWarning("Cliente 'Invitado' (ID: {GuestId}) no encontrado en la base de datos local. Creándolo...", GUEST_CLIENT_ID);
                var guestClienteCrear = new ClienteCrearDto
                {
                    Dni = GUEST_CLIENT_DNI,
                    Titular = GUEST_CLIENT_TITULAR
                };
                var (newGuest, guestError) = await CreateClienteAsync(guestClienteCrear);
                if (newGuest != null)
                {
                    guestClient = newGuest;
                    _logger.LogInformation("Cliente 'Invitado' creado exitosamente con ID: {Id}", newGuest.Id);
                }
                else
                {
                    _logger.LogError("Fallo al crear el cliente 'Invitado': {Error}", guestError);
                    // Fallback to a hardcoded DTO if guest client creation fails
                    return new ClienteDto { Id = GUEST_CLIENT_ID, Dni = GUEST_CLIENT_DNI, Titular = GUEST_CLIENT_TITULAR };
                }
            }

            return _mapper.Map<ClienteDto>(guestClient);
        }
    }
}