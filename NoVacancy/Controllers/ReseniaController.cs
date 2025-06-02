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
        public async Task<IActionResult> Create(int idProducto, int idPedido)
        {
            var producto = await _context.Productos.FindAsync(idProducto);
            if (producto == null) return NotFound();
            var model = new Resenia { idProducto = idProducto, idPedido = idPedido };
            ViewBag.ProductoNombre = producto.nombre;
            ViewBag.idPedido = idPedido;
            return View(model);
        }

        // POST: ReseniaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Resenia model)
        {
            if (ModelState.IsValid)
            {
                // Validar que el pedido exista
                var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.idPedido == model.idPedido);
                if (!pedidoExiste)
                {
                    ModelState.AddModelError("", "El pedido no existe.");
                }
                else
                {
                    // Validar que no exista ya una reseña para ese producto en ese pedido
                    var existe = await _context.Resenias.AnyAsync(r => r.idProducto == model.idProducto && r.idPedido == model.idPedido);
                    if (existe)
                    {
                        ModelState.AddModelError("", "Ya existe una reseña para este producto en este pedido.");
                    }
                    else
                    {
                        _context.Resenias.Add(model);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("DetallesPedido", "Usuario", new { id = model.idPedido });
                    }
                }
            }
            var producto = await _context.Productos.FindAsync(model.idProducto);
            ViewBag.ProductoNombre = producto?.nombre;
            ViewBag.idPedido = model.idPedido;
            return View(model);
        }

        // GET: ReseniaController/Edit
        public async Task<IActionResult> Edit(int idProducto, int idPedido)
        {
            var resenia = await _context.Resenias.FirstOrDefaultAsync(r => r.idProducto == idProducto && r.idPedido == idPedido);
            if (resenia == null) return NotFound();
            var producto = await _context.Productos.FindAsync(idProducto);
            ViewBag.ProductoNombre = producto?.nombre;
            ViewBag.idPedido = idPedido;
            return View(resenia);
        }

        // POST: ReseniaController/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Resenia model)
        {
            if (ModelState.IsValid)
            {
                var resenia = await _context.Resenias.FirstOrDefaultAsync(r => r.idResenia == model.idResenia);
                if (resenia == null) return NotFound();
                resenia.resenia = model.resenia;
                resenia.calificacion = model.calificacion;
                await _context.SaveChangesAsync();
                return RedirectToAction("DetallesPedido", "Usuario", new { id = resenia.idPedido });
            }
            var producto = await _context.Productos.FindAsync(model.idProducto);
            ViewBag.ProductoNombre = producto?.nombre;
            ViewBag.idPedido = model.idPedido;
            return View(model);
        }

        // GET: ReseniaController/Delete
        public async Task<IActionResult> Delete(int idProducto, int idPedido)
        {
            var resenia = await _context.Resenias.FirstOrDefaultAsync(r => r.idProducto == idProducto && r.idPedido == idPedido);
            if (resenia == null) return NotFound();
            var producto = await _context.Productos.FindAsync(idProducto);
            ViewBag.ProductoNombre = producto?.nombre;
            ViewBag.idPedido = idPedido;
            return View(resenia);
        }

        // POST: ReseniaController/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idResenia)
        {
            var resenia = await _context.Resenias.FindAsync(idResenia);
            if (resenia == null) return NotFound();
            int idPedido = resenia.idPedido;
            _context.Resenias.Remove(resenia);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetallesPedido", "Usuario", new { id = idPedido });
        }
    }
}
