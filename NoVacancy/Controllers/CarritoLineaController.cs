using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

/*
 * 
 * NOTA IMPORTANTÍSIMA:
 * C.Id (con la I mayúscula) hace referencia
 * al ID del Usuario, Identity la maneja así,
 * y además es un String, no un int
 * 
 * Maldigo a Identity
 * 
 */

namespace NoVacancy.Controllers
{
    public class CarritoLineaController : Controller
    {
        public IActionResult Shopping_cart()
        {
            var carritoLineas = new List<CarritoLinea>(); // Replace with actual data retrieval logic
            return View(carritoLineas);
        }

        private readonly NoVacancyDbContext _context;
        public CarritoLineaController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: CarritoLinea/ShoppingCart
        public async Task<IActionResult> ShoppingCart()
        {
            string? usuarioId = HttpContext.Session.GetString("Id");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return View(new List<CarritoLinea>());

            var lineas = await _context.CarritosLineas
                .Where(l => l.idCarrito == carrito.idCarrito)
                .Include(l => l.Producto)
                .ToListAsync();

            return View("~/Views/Home/Shopping_cart.cshtml", lineas);
        }

        // POST: CarritoLinea/Add
        [HttpPost]
        public async Task<IActionResult> Add(int idProducto, int cantidad = 1)
        {
            string? usuarioId = HttpContext.Session.GetString("Id");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
            {
                carrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(carrito);
                await _context.SaveChangesAsync();
            }

            var linea = await _context.CarritosLineas.FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == idProducto);
            if (linea != null)
            {
                linea.cantidad += cantidad;
                _context.CarritosLineas.Update(linea);
            }
            else
            {
                linea = new CarritoLinea { idCarrito = carrito.idCarrito, idProducto = idProducto, cantidad = cantidad };
                _context.CarritosLineas.Add(linea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Shopping_cart");
        }

        // POST: CarritoLinea/Update
        [HttpPost]
        public async Task<IActionResult> Update(int idProducto, int cantidad)
        {
            string? usuarioId = HttpContext.Session.GetString("Id");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return RedirectToAction("Shopping_cart");

            var linea = await _context.CarritosLineas.FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == idProducto);
            if (linea != null)
            {
                linea.cantidad = cantidad;
                _context.CarritosLineas.Update(linea);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Shopping_cart");
        }

        // POST: CarritoLinea/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int idProducto)
        {
            string? usuarioId = HttpContext.Session.GetString("Id");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return RedirectToAction("Shopping_cart");

            var linea = await _context.CarritosLineas.FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == idProducto);
            if (linea != null)
            {
                _context.CarritosLineas.Remove(linea);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Shopping_cart");
        }

        // POST: CarritoLinea/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            string? usuarioId = HttpContext.Session.GetString("Id");
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito != null)
            {
                var lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito);
                _context.CarritosLineas.RemoveRange(lineas);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Shopping_cart");
        }
    }
}
