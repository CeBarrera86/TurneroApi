using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.DTOs;
using TurneroApi.Models;
using AutoMapper;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly TurneroDbContext _context;
        private readonly IMapper _mapper;

        public ClienteController(TurneroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClienteDto>>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();
            var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
            return Ok(clientesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteDto>> GetCliente(ulong id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
                return NotFound();

            return _mapper.Map<ClienteDto>(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDto>> PostCliente(ClienteDto clienteDto)
        {
            var cliente = _mapper.Map<Cliente>(clienteDto);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            var createdDto = _mapper.Map<ClienteDto>(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(ulong id, ClienteDto clienteDto)
        {
            if (id != clienteDto.Id)
                return BadRequest();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _mapper.Map(clienteDto, cliente); // Actualiza la entidad con los valores del DTO

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(ulong id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(ulong id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}