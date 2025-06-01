using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    public class ReseniaController : Controller
    {
        private readonly NoVacancyDbContext _context;
        public ReseniaController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: ReseniaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReseniaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReseniaController/Create
        public async Task<IActionResult> Create(int idProducto)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null) return NotFound();
            var model = new Resenia { idProducto = idProducto };
            ViewBag.ProductoNombre = producto.nombre;
            return View(model);
        }

        // POST: ReseniaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resenia model)
        {
            if (ModelState.IsValid)
            {
                _context.Resenias.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Historial", "Pedido");
            }
            var producto = await _context.Productos.FindAsync(model.idProducto);
            ViewBag.ProductoNombre = producto?.nombre;
            return View(model);
        }

        // GET: ReseniaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReseniaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReseniaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReseniaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
