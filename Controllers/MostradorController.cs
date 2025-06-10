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
    public class MostradorController : ControllerBase
    {
        private readonly TurneroDbContext _context;

        public MostradorController(TurneroDbContext context)
        {
            _context = context;
        }

        // GET: api/Mostrador
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mostrador>>> GetMostradores()
        {
            return await _context.Mostradores.ToListAsync();
        }

        // GET: api/Mostrador/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mostrador>> GetMostrador(uint id)
        {
            var mostrador = await _context.Mostradores.FindAsync(id);

            if (mostrador == null)
            {
                return NotFound();
            }

            return mostrador;
        }

        // PUT: api/Mostrador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMostrador(uint id, Mostrador mostrador)
        {
            if (id != mostrador.Id)
            {
                return BadRequest();
            }

            _context.Entry(mostrador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MostradorExists(id))
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

        // POST: api/Mostrador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mostrador>> PostMostrador(Mostrador mostrador)
        {
            _context.Mostradores.Add(mostrador);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMostrador", new { id = mostrador.Id }, mostrador);
        }

        // DELETE: api/Mostrador/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMostrador(uint id)
        {
            var mostrador = await _context.Mostradores.FindAsync(id);
            if (mostrador == null)
            {
                return NotFound();
            }

            _context.Mostradores.Remove(mostrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MostradorExists(uint id)
        {
            return _context.Mostradores.Any(e => e.Id == id);
        }
    }
}
