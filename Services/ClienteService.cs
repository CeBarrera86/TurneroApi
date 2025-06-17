using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Services
{
    public class ClienteService : IClienteService
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;
        // private readonly IClienteRemotoService _clienteRemotoService; // Inyectar si tienes un servicio remoto

        public ClienteService(TurneroDbContext context, IMapper mapper /*, IClienteRemotoService clienteRemotoService */)
        {
            _context = context;
            _mapper = mapper;
            // _clienteRemotoService = clienteRemotoService;
        }

        public async Task<Cliente?> GetClienteByDniAsync(string dni)
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
                                 .OrderBy(c => c.Titular) // O el orden que prefieras
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }

        public async Task<(Cliente? cliente, string? errorMessage)> CreateClienteAsync(ClienteCrearDto clienteCrearDto)
        {
            // Validación de unicidad: DNI debe ser único
            var existingCliente = await _context.Clientes.AnyAsync(c => c.Dni == clienteCrearDto.Dni);
            if (existingCliente)
            {
                return (null, $"Ya existe un cliente con el DNI '{clienteCrearDto.Dni}'.");
            }

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

        // --- Lógica adicional para el Totem (integraría la búsqueda remota aquí o en un orquestador) ---
        /*
        public async Task<(Cliente? clienteLocal, string? errorMessage)> GetOrCreateClienteFromRemoteAsync(string dni)
        {
            // 1. Buscar en la base de datos local
            var clienteLocal = await GetClienteByDniAsync(dni);
            if (clienteLocal != null)
            {
                return (clienteLocal, null); // Cliente ya existe localmente
            }

            // 2. Si no se encuentra localmente, buscar en la base de datos remota
            var clienteRemoto = await _clienteRemotoService.BuscarClienteEnBaseRemotaAsync(dni);
            if (clienteRemoto == null)
            {
                return (null, "Cliente no encontrado en la base de datos local ni remota.");
            }

            // 3. Si se encuentra remotamente, crear una nueva tupla en la base de datos local
            var newClienteDto = new ClienteCrearDto
            {
                Dni = clienteRemoto.Dni,
                Titular = clienteRemoto.Titular
            };

            var (createdCliente, createError) = await CreateClienteAsync(newClienteDto);
            if (createdCliente == null)
            {
                return (null, $"Error al crear el cliente localmente desde la base remota: {createError}");
            }

            return (createdCliente, null);
        }
        */
    }

    // Opcional: Una implementación dummy del servicio remoto para testing o desarrollo inicial
    /*
    public class ClienteRemotoService : IClienteRemotoService
    {
        public Task<IClienteRemotoService.ClienteRemotoDto?> BuscarClienteEnBaseRemotaAsync(string dni)
        {
            // Simulamos una búsqueda remota. En un caso real, esto sería una llamada a otra API, etc.
            if (dni == "12345678" || dni == "87654321")
            {
                return Task.FromResult(new IClienteRemotoService.ClienteRemotoDto
                {
                    Dni = dni,
                    Titular = dni == "12345678" ? "Juan Perez Remoto" : "Maria Lopez Remoto"
                });
            }
            return Task.FromResult<IClienteRemotoService.ClienteRemotoDto?>(null);
        }
    }
    */
}