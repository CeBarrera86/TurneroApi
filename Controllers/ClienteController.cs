using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TurneroApi.DTOs.Cliente;
using TurneroApi.Interfaces;
using TurneroApi.Utils;

namespace TurneroApi.Controllers;

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
  [EnableRateLimiting("public")]
  public async Task<ActionResult<ClienteDto>> GetClienteByDni(string dni)
  {
    if (string.IsNullOrWhiteSpace(dni))
      return BadRequest("El DNI no puede estar vacío.");

    var clienteDto = await _clienteService.ObtenerClientePorDni(dni);
    if (clienteDto == null) return NotFound();

    return Ok(clienteDto);
  }

  // POST: api/Cliente
  [HttpPost]
  [Authorize(Policy = "crear_cliente")]
  public async Task<ActionResult<ClienteDto>> PostCliente([FromBody] ClienteCrearDto clienteCrearDto)
  {
    if (!ModelState.IsValid) return BadRequest(ModelState);

    var (createdCliente, errorMessage) = await _clienteService.CreateClienteAsync(clienteCrearDto);
    if (createdCliente == null) return BadRequest(new { message = errorMessage });

    var clienteDto = new ClienteDto
    {
      Id = createdCliente.Id,
      Dni = createdCliente.Dni,
      Titular = createdCliente.Titular
    };

    return CreatedAtAction(nameof(GetClienteByDni), new { dni = clienteDto.Dni }, clienteDto);
  }

  // GET: api/Cliente?page=1&pageSize=10
  [HttpGet]
  [Authorize(Policy = "ver_cliente")]
  public async Task<ActionResult<PagedResponse<ClienteDto>>> GetClientes(int page = 1, int pageSize = 10)
  {
    if (!PaginationHelper.IsValid(page, pageSize, out var message))
    {
      return BadRequest(new { message });
    }

    var result = await _clienteService.GetClientesAsync(page, pageSize);
    return Ok(new PagedResponse<ClienteDto>(result.Items, page, pageSize, result.Total));
  }
}
