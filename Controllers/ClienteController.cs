using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;
using TurneroApi.Models;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, IMapper mapper)
        {
            _clienteService = clienteService;
            _mapper = mapper;
        }

        // GET: api/Cliente/dni/{dni} (Now handles local, remote, and guest fallback)
        [HttpGet("dni/{dni}")]
        [AllowAnonymous]
        public async Task<ActionResult<ClienteDto>> GetClienteByDni(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                return BadRequest("El DNI no puede estar vacío.");
            }

            var clienteDto = await _clienteService.ObtenerClientePorDni(dni);

            return Ok(clienteDto);
        }

        // POST: api/Cliente (for creating a client, e.g., from an admin panel)
        [HttpPost]
        [Authorize(Roles = "Admin")] // Only an administrator or an automated process can create clients localmente
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

            return CreatedAtAction(nameof(GetClienteByDni), new { dni = clienteDto.Dni }, clienteDto);
            // Changed from GetCliente (by id) to GetClienteByDni (by dni) as GetCliente is removed.
            // Note: CreatedAtAction requires a route name or action name that maps to a GET verb for the created resource.
            // If GetCliente (by ID) is entirely removed, you might need to adjust this, or just return an Ok(clienteDto).
            // For now, I've updated it to refer to GetClienteByDni by DNI.
        }
    }
}