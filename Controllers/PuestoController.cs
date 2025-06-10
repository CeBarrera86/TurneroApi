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
    public class PuestoController : ControllerBase
    {
        private readonly TurneroDbContext _context;

        public PuestoController(TurneroDbContext context)
        {
            _context = context;
        }

        // GET: api/Puesto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Puesto>>> GetPuestos()
        {
            return await _context.Puestos.ToListAsync();
        }

        // GET: api/Puesto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Puesto>> GetPuesto(uint id)
        {
            var puesto = await _context.Puestos.FindAsync(id);

            if (puesto == null)
            {
                return NotFound();
            }

            return puesto;
        }

        // PUT: api/Puesto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPuesto(uint id, Puesto puesto)
        {
            if (id != puesto.Id)
            {
                return BadRequest();
            }

            _context.Entry(puesto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuestoExists(id))
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

        // POST: api/Puesto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Puesto>> PostPuesto(Puesto puesto)
        {
            _context.Puestos.Add(puesto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPuesto", new { id = puesto.Id }, puesto);
        }

        // DELETE: api/Puesto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePuesto(uint id)
        {
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }

            _context.Puestos.Remove(puesto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PuestoExists(uint id)
        {
            return _context.Puestos.Any(e => e.Id == id);
        }
    }
}
