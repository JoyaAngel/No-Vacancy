using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly NoVacancyDbContext _context;
        public PedidoController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: api/PedidoApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _context.Pedidos.Include(p => p.Carrito).ToListAsync();
            return Ok(pedidos);
        }

        // GET: api/PedidoApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var pedido = await _context.Pedidos.Include(p => p.Carrito).FirstOrDefaultAsync(p => p.idPedido == id);
            if (pedido == null)
                return NotFound();
            return Ok(pedido);
        }

        // POST: api/PedidoApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Pedido pedido)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Calcular el monto total del carrito
            var lineas = await _context.CarritosLineas
                .Where(l => l.idCarrito == pedido.idCarrito)
                .Include(l => l.Producto)
                .ToListAsync();
            double montoTotal = lineas.Sum(l => l.cantidad * (l.Producto != null ? l.Producto.precio : 0));

            // Crear el detalle
            var detalle = new Detalle
            {
                monto = montoTotal,
                idPedido = pedido.idPedido
            };
            _context.Set<Detalle>().Add(detalle);
            await _context.SaveChangesAsync();

            return Ok(pedido);
        }

        // PUT: api/PedidoApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pedido pedido)
        {
            if (id != pedido.idPedido)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _context.Entry(pedido).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pedidos.Any(e => e.idPedido == id))
                    return NotFound();
                else
                    throw;
            }
            return NoContent();
        }

        // DELETE: api/PedidoApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();
            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
