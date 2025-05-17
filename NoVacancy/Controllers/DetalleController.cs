using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoVacancy.Models;
using NoVacancy.Data;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleController : ControllerBase
    {
        private readonly NoVacancyDbContex _context;

        public DetalleController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: api/Detalle
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Detalle>>> GetDetalles()
        {
            return await _context.Set<Detalle>().ToListAsync();
        }

        // GET: api/Detalle/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Detalle>> GetDetalle(int id)
        {
            var detalle = await _context.Set<Detalle>().FirstOrDefaultAsync(d => d.idDetalle == id);
            if (detalle == null)
                return NotFound();
            return detalle;
        }

        // POST: api/Detalle
        [HttpPost]
        public async Task<ActionResult<Detalle>> CreateDetalle([FromBody] Detalle detalle)
        {
            _context.Set<Detalle>().Add(detalle);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDetalle), new { id = detalle.idDetalle }, detalle);
        }

        // PUT: api/Detalle/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDetalle(int id, [FromBody] Detalle detalle)
        {
            if (id != detalle.idDetalle)
                return BadRequest();
            _context.Entry(detalle).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Set<Detalle>().Any(e => e.idDetalle == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/Detalle/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.Set<Detalle>().FirstOrDefaultAsync(d => d.idDetalle == id);
            if (detalle == null)
                return NotFound();
            _context.Set<Detalle>().Remove(detalle);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
