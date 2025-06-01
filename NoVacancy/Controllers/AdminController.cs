using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NoVacancy.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
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