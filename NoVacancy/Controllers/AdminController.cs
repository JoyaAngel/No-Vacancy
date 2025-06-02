using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;


namespace NoVacancy.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {

        private readonly NoVacancyDbContext _context;

        public AdminController(NoVacancyDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .ToListAsync();

            return View(productos);
        }

        [HttpGet] // Muestra el formulario
        public IActionResult Edit(int id)
        {
            // Aquí cargarías los datos del producto desde la BD usando el "id"
            return View();
        }

        [HttpPost]
        public IActionResult Edit(
            string titulo,
            string descripcion,
            IFormFile imagen,
            string categoria,
            string color,
            string talla,
            decimal precio)
        {
            // Lógica para guardar los datos (ej. en una base de datos)
            return RedirectToAction("Index"); // Redirige al listado
        }
    }
}