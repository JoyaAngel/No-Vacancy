using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ImagenController : ControllerBase
    {
        private readonly NoVacancyDbContex _context;
        public ImagenController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: api/Imagen
        [HttpGet]
        public IActionResult GetImagenes()
        {
            var imagenes = _context.Imagenes.Include(i => i.Producto).ToList();
            return Ok(imagenes);
        }

        // GET: api/Imagen/{id}
        [HttpGet("{id}")]
        public IActionResult GetImagen(int id)
        {
            var imagen = _context.Imagenes.Include(i => i.Producto).FirstOrDefault(i => i.idImagen == id);
            if (imagen == null)
                return NotFound();
            return Ok(imagen);
        }

        // POST: api/Imagen
        [HttpPost]
        public IActionResult Create([FromBody] Imagen imagen)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.Imagenes.Add(imagen);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetImagen), new { id = imagen.idImagen }, imagen);
        }

        // PUT: api/Imagen/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Imagen imagen)
        {
            if (id != imagen.idImagen)
                return BadRequest();
            if (!_context.Imagenes.Any(i => i.idImagen == id))
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.Entry(imagen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Imagen/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var imagen = _context.Imagenes.Find(id);
            if (imagen == null)
                return NotFound();
            _context.Imagenes.Remove(imagen);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
