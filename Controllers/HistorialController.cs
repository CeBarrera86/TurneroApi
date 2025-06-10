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
    public class HistorialController : ControllerBase
    {
        private readonly TurneroDbContext _context;

        public HistorialController(TurneroDbContext context)
        {
            _context = context;
        }

        // GET: api/Historial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Historial>>> GetHistoriales()
        {
            return await _context.Historiales.ToListAsync();
        }

        // GET: api/Historial/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Historial>> GetHistorial(ulong id)
        {
            var historial = await _context.Historiales.FindAsync(id);

            if (historial == null)
            {
                return NotFound();
            }

            return historial;
        }

        // PUT: api/Historial/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorial(ulong id, Historial historial)
        {
            if (id != historial.Id)
            {
                return BadRequest();
            }

            _context.Entry(historial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorialExists(id))
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

        // POST: api/Historial
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Historial>> PostHistorial(Historial historial)
        {
            _context.Historiales.Add(historial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistorial", new { id = historial.Id }, historial);
        }

        // DELETE: api/Historial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorial(ulong id)
        {
            var historial = await _context.Historiales.FindAsync(id);
            if (historial == null)
            {
                return NotFound();
            }

            _context.Historiales.Remove(historial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistorialExists(ulong id)
        {
            return _context.Historiales.Any(e => e.Id == id);
        }
    }
}
