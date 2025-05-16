using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoriaController : ControllerBase
    {
        private readonly NoVacancyDbContex _context;

        public CategoriaController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: api/Categoria
        [HttpGet]
        public IActionResult GetCategorias()
        {
            var categorias = _context.Categorias.ToList();
            return Ok(categorias);
        }

        // POST: api/Categoria
        [HttpPost]
        public IActionResult Create([FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }

        // PUT: api/Categoria/{id}
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Categoria categoria)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingCategoria = _context.Categorias.Find(id);

            if (existingCategoria == null)
                return NotFound();

            existingCategoria.nombre = categoria.nombre;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Categoria/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var categoria = _context.Categorias.Find(id);
            if (categoria == null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
