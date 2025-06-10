using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TurneroApi.Data;
using TurneroApi.Models;

namespace TurneroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadoController : ControllerBase
    {
        private readonly TurneroDbContext _context;

        public EstadoController(TurneroDbContext context)
        {
            _context = context;
        }

        // GET: api/Estado
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estado>>> GetEstados()
        {
            return await _context.Estados.ToListAsync();
        }

        // GET: api/Estado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estado>> GetEstado(uint id)
        {
            var estado = await _context.Estados.FindAsync(id);

            if (estado == null)
            {
                return NotFound();
            }

            return estado;
        }

        // PUT: api/Estado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstado(uint id, Estado estado)
        {
            if (id != estado.Id)
            {
                return BadRequest();
            }

            _context.Entry(estado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstadoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Estado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Estado>> PostEstado(Estado estado)
        {
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEstado", new { id = estado.Id }, estado);
        }

        // DELETE: api/Estado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstado(uint id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null)
            {
                return NotFound();
            }

            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EstadoExists(uint id)
        {
            return _context.Estados.Any(e => e.Id == id);
        }
    }
}
