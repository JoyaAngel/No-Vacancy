using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly NoVacancyDbContex _context;
        public ColorController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: api/Color
        [HttpGet]
        public IActionResult GetColores()
        {
            var colores = _context.Colores.ToList();
            return Ok(colores);
        }

        // POST: api/Color
        [HttpPost]
        public IActionResult Create([FromBody] Color color)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Colores.Add(color);
            _context.SaveChanges();
            return Ok(color);
        }

        // PUT: api/Color/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Color color)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingColor = _context.Colores.Find(id);
            if (existingColor == null)
                return NotFound();

            existingColor.nombre = color.nombre;
            existingColor.codigo = color.codigo;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Color/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var color = _context.Colores.Find(id);
            if (color == null)
                return NotFound();

            _context.Colores.Remove(color);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
