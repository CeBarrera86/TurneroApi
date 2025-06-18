using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurneroApi.DTOs;
using TurneroApi.Interfaces;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: api/Cliente/{dni}
        [HttpGet("{dni}")]
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
    }
}