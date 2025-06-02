using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using NoVacancy.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
        private readonly NoVacancyDbContext _context;
        public CarritoLineaController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: CarritoLinea/Shopping_cart
        public async Task<IActionResult> Shopping_cart()
        {
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            // Buscar el carrito más reciente del usuario que NO esté asociado a un Pedido
            var carrito = await _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefaultAsync();
            if (carrito == null)
                return View(new List<CarritoLineaViewModel>());

            // Obtener las líneas SOLO de ese carrito
            var lineas = await _context.CarritosLineas
                .Where(l => l.idCarrito == carrito.idCarrito)
                .Include(l => l.Producto)
                .ToListAsync();

            // Obtener ids de productos
            var productoIds = lineas.Select(l => l.idProducto).ToList();
            // Obtener imágenes asociadas a los productos
            var imagenes = await _context.Imagenes
                .Where(img => productoIds.Contains(img.idProducto))
                .GroupBy(img => img.idProducto)
                .Select(g => new { idProducto = g.Key, Imagen = g.FirstOrDefault() })
                .ToListAsync();

            // Cargar Color y Talla manualmente para cada producto
            foreach (var linea in lineas)
            {
                if (linea.Producto != null)
                {
                    linea.Producto.Color = await _context.Colores.FirstOrDefaultAsync(c => c.idColor == linea.Producto.idColor);
                    linea.Producto.Talla = await _context.Tallas.FirstOrDefaultAsync(t => t.idTalla == linea.Producto.idTalla);
                }
            }

            var viewModel = lineas.Select(linea => {
                var imgObj = imagenes.FirstOrDefault(i => i.idProducto == linea.idProducto)?.Imagen;
                // Fallback: si no hay imagen para la variante, busca una imagen de otro producto con el mismo nombre
                if (imgObj == null && linea.Producto != null && !string.IsNullOrEmpty(linea.Producto.nombre))
                {
                    var productoConImagen = _context.Productos
                        .Where(p => p.nombre == linea.Producto.nombre)
                        .Join(_context.Imagenes, p => p.idProducto, im => im.idProducto, (p, im) => im)
                        .FirstOrDefault();
                    if (productoConImagen != null)
                        imgObj = productoConImagen;
                }
                return new CarritoLineaViewModel
                {
                    CarritoLinea = linea,
                    ImagenPrincipal = imgObj != null ? $"/images/productos/{imgObj.nombre}" : "/images/default-150x150.png",
                    Color = linea.Producto?.Color?.nombre ?? "-",
                    Talla = linea.Producto?.Talla?.nombre ?? "-"
                };
            }).ToList();

            return View("Shopping_cart", viewModel);
        }

        // POST: CarritoLinea/Add
        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Add(int idProducto, int cantidad = 1)
        {
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefaultAsync();
            if (carrito == null)
            {
                carrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(carrito);
                await _context.SaveChangesAsync();
            }

            // Validar que el producto existe
            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.idProducto == idProducto);
            if (producto == null)
            {
                System.Diagnostics.Debug.WriteLine($"[CarritoLineaController] Producto con id {idProducto} no existe.");
                TempData["Error"] = "El producto seleccionado no existe.";
                return RedirectToAction("Shopping_cart");
            }

            var linea = await _context.CarritosLineas.FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == idProducto);
            int nuevaCantidad = cantidad;
            if (linea != null)
                nuevaCantidad = linea.cantidad + cantidad;

            if (producto.limite.HasValue && nuevaCantidad > producto.limite.Value)
            {
                TempData["Error"] = $"No puedes agregar más de {producto.limite.Value} unidades de este producto.";
                return RedirectToAction("Shopping_cart");
            }

            if (linea != null)
            {
                linea.cantidad = nuevaCantidad;
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
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Update(int idProducto, int cantidad)
        {
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefaultAsync();
            if (carrito == null)
                return RedirectToAction("Shopping_cart");

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.idProducto == idProducto);
            if (producto == null)
            {
                TempData["Error"] = "El producto seleccionado no existe.";
                return RedirectToAction("Shopping_cart");
            }

            if (producto.limite.HasValue && cantidad > producto.limite.Value)
            {
                TempData["Error"] = $"No puedes seleccionar más de {producto.limite.Value} unidades de este producto.";
                return RedirectToAction("Shopping_cart");
            }

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
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Delete(int idProducto)
        {
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefaultAsync();
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
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login", "Usuario");

            var carrito = await _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefaultAsync();
            if (carrito != null)
            {
                var lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito);
                _context.CarritosLineas.RemoveRange(lineas);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Shopping_cart");
        }

        // GET: CarritoLinea/Invoice
        public IActionResult Invoice()
        {
            return View();
        }
    }
}
