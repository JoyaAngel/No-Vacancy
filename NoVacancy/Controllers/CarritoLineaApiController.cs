using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoLineaApiController : ControllerBase
    {
        private readonly NoVacancyDbContex _context;
        public CarritoLineaApiController(NoVacancyDbContex context)
        {
            _context = context;
        }

        //obtener id del usuario autenticado
        private string? ObtenerUsuarioId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        // GET: api/CarritoLineaApi
        [HttpGet]
        public async Task<IActionResult> GetCarrito()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return Ok(new List<CarritoLinea>());

            var lineas = await _context.CarritosLineas
                .Where(l => l.idCarrito == carrito.idCarrito)
                .Include(l => l.Producto)
                .ToListAsync();

            return Ok(lineas);
        }

        // POST: api/CarritoLineaApi/Add
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] CarritoLinea model)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
            {
                carrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(carrito);
                await _context.SaveChangesAsync();
            }

            var linea = await _context.CarritosLineas
                .FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == model.idProducto);

            if (linea != null)
            {
                linea.cantidad += model.cantidad;
                _context.CarritosLineas.Update(linea);
            }
            else
            {
                linea = new CarritoLinea
                {
                    idCarrito = carrito.idCarrito,
                    idProducto = model.idProducto,
                    cantidad = model.cantidad
                };
                _context.CarritosLineas.Add(linea);
            }

            await _context.SaveChangesAsync();
            return Ok(linea);
        }

        // PUT: api/CarritoLineaApi/Update
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CarritoLinea model)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return NotFound();

            var linea = await _context.CarritosLineas
                .FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == model.idProducto);

            if (linea != null)
            {
                linea.cantidad = model.cantidad;
                _context.CarritosLineas.Update(linea);
                await _context.SaveChangesAsync();
                return Ok(linea);
            }

            return NotFound();
        }

        // DELETE: api/CarritoLineaApi/Delete/5
        [HttpDelete("Delete/{idProducto}")]
        public async Task<IActionResult> Delete(int idProducto)
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito == null)
                return NotFound();

            var linea = await _context.CarritosLineas
                .FirstOrDefaultAsync(l => l.idCarrito == carrito.idCarrito && l.idProducto == idProducto);

            if (linea != null)
            {
                _context.CarritosLineas.Remove(linea);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        // DELETE: api/CarritoLineaApi/Clear
        [HttpDelete("Clear")]
        public async Task<IActionResult> Clear()
        {
            var usuarioId = ObtenerUsuarioId();
            if (usuarioId == null) return Unauthorized();

            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.Id == usuarioId);
            if (carrito != null)
            {
                var lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito);
                _context.CarritosLineas.RemoveRange(lineas);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }
    }
}
