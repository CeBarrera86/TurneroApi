using AutoMapper;
using Microsoft.AspNetCore.Authorization; // Add if you plan to use authorization
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models; // If you still need direct model references for some reason

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;
        // private readonly IClienteRemotoService _clienteRemotoService; // Si decides usarlo directamente aquí o en el service

        public ClienteController(IClienteService clienteService, IMapper mapper /*, IClienteRemotoService clienteRemotoService */)
        {
            _clienteService = clienteService;
            _mapper = mapper;
            // _clienteRemotoService = clienteRemotoService;
        }

        // GET: api/Cliente?page=1&pageSize=10 (opcional, si necesitas listar clientes)
        [HttpGet]
        [Authorize(Roles = "Admin")] // Probablemente solo Admins pueden listar todos los clientes
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var clientes = await _clienteService.GetClientesAsync(page, pageSize);
            var clientesDto = _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        // GET: api/Cliente/{id} (para obtener por ID de la base de datos local)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Usuario")] // Puede que sea útil para otros servicios internos
        public async Task<ActionResult<ClienteDto>> GetCliente(ulong id)
        {
            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            var clienteDto = _mapper.Map<ClienteDto>(cliente);
            return Ok(clienteDto);
        }

        // GET: api/Cliente/dni/{dni} (el endpoint principal para buscar clientes)
        [HttpGet("dni/{dni}")]
        // [Authorize(Roles = "TotemUser, Admin, Usuario")] // Ajusta los roles para quien usa el totem
        public async Task<ActionResult<ClienteDto>> GetClienteByDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest("El DNI no puede estar vacío.");
            }

            // 1. Buscar en la base de datos local
            var clienteLocal = await _clienteService.GetClienteByDniAsync(dni);
            if (clienteLocal != null)
            {
                return Ok(_mapper.Map<ClienteDto>(clienteLocal));
            }

            // 2. Si no se encuentra localmente, buscar en la base de datos remota
            // Esta lógica de búsqueda y creación local podría estar en un orquestador o directamente en ClienteService.
            // Para mantener el controlador delgado, es mejor que el servicio principal maneje la orquestación.

            // Ejemplo conceptual si la orquestación está en el servicio:
            /*
            var (cliente, errorMessage) = await _clienteService.GetOrCreateClienteFromRemoteAsync(dni);
            if (cliente == null)
            {
                return NotFound(new { message = errorMessage });
            }
            return Ok(_mapper.Map<ClienteDto>(cliente));
            */

            // Para este ejemplo, si no está localmente, indicamos que no se encontró aquí.
            // La lógica de buscar en remoto y crear localmente se externalizaría.
            return NotFound($"Cliente con DNI '{dni}' no encontrado localmente.");
        }


        // POST: api/Cliente (para crear un cliente, asumimos que solo se usa internamente o por un sistema específico)
        [HttpPost]
        [Authorize(Roles = "Admin")] // Solo un administrador o un proceso automatizado puede crear clientes localmente
        public async Task<ActionResult<ClienteDto>> PostCliente([FromBody] ClienteCrearDto clienteCrearDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (createdCliente, errorMessage) = await _clienteService.CreateClienteAsync(clienteCrearDto);

            if (createdCliente == null)
            {
                return BadRequest(new { message = errorMessage });
            }

            var clienteDto = _mapper.Map<ClienteDto>(createdCliente);
            return CreatedAtAction(nameof(GetCliente), new { id = clienteDto.Id }, clienteDto);
        }

        // Los métodos PUT, PATCH y DELETE son eliminados ya que no se gestionan actualizaciones ni eliminaciones directas de clientes locales.
    }
}