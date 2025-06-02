using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using NoVacancy.Services;
using Microsoft.AspNetCore.Identity;

namespace NoVacancy.Controllers
{
    
    public class PedidoController : Controller
    {
        private readonly NoVacancyDbContext _context;
        private readonly EmailService _emailService;
        private readonly UserManager<Usuario> _userManager;
        public PedidoController(NoVacancyDbContext context, EmailService emailService, UserManager<Usuario> userManager)
        {
            _context = context;
            _emailService = emailService;
            _userManager = userManager;
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
        public async Task<IActionResult> Create([FromBody] Pedido pedido, [FromBody] List<CarritoLinea> productosPedido)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // 1. Crear el carrito si no existe
            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.idCarrito == pedido.idCarrito);
            if (carrito == null)
            {
                carrito = new CarritoCabecera { Id = pedido.Carrito?.Id };
                _context.CarritosCabecera.Add(carrito);
                await _context.SaveChangesAsync();
                pedido.idCarrito = carrito.idCarrito;
            }
            // 2. Crear las líneas del carrito si no existen
            var lineasExistentes = await _context.CarritosLineas.Where(l => l.idCarrito == pedido.idCarrito).ToListAsync();
            if (lineasExistentes.Count == 0 && productosPedido != null)
            {
                foreach (var prod in productosPedido)
                {
                    var nuevaLinea = new CarritoLinea
                    {
                        idCarrito = pedido.idCarrito,
                        idProducto = prod.idProducto,
                        cantidad = prod.cantidad
                    };
                    _context.CarritosLineas.Add(nuevaLinea);
                }
                await _context.SaveChangesAsync();
            }
            // 3. Crear el pedido
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

            // Restar stock de los productos comprados
            foreach (var linea in lineas)
            {
                var productoDb = await _context.Productos.FirstOrDefaultAsync(p => p.idProducto == linea.idProducto);
                if (productoDb != null)
                {
                    productoDb.cantidad -= linea.cantidad;
                    if (productoDb.cantidad < 0)
                        productoDb.cantidad = 0;
                    _context.Productos.Update(productoDb);
                }
            }
            await _context.SaveChangesAsync();

            // Enviar email de confirmación de pedido
            var usuarioId = pedido.Carrito?.Id ?? carrito.Id;
            if (!string.IsNullOrEmpty(usuarioId))
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId);
                if (usuario != null && !string.IsNullOrEmpty(usuario.Email))
                {
                    string subject = "Confirmación de tu pedido - No-Vacancy";
                    string body = $"<h2>¡Gracias por tu compra, {usuario.Nombre}!</h2><p>Tu pedido #{pedido.idPedido} ha sido registrado exitosamente.</p><p>Monto total: <strong>{detalle.monto:C}</strong></p>";
                    await _emailService.SendEmailAsync(usuario.Email, subject, body);
                }
            }

            return Ok(pedido);
        }

        // POST: api/PedidoApi/CreateManual
        [HttpPost("CreateManual")]
        public async Task<IActionResult> CreateManual([FromBody] Pedido pedido, [FromBody] List<CarritoLinea> productosPedido)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // 1. Crear el carrito si no existe
            var carrito = await _context.CarritosCabecera.FirstOrDefaultAsync(c => c.idCarrito == pedido.idCarrito);
            if (carrito == null)
            {
                var userId = pedido.Carrito?.Id ?? productosPedido.FirstOrDefault()?.Carrito?.Id;
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("No se proporcionó usuario para el carrito");
                carrito = new CarritoCabecera { Id = userId };
                _context.CarritosCabecera.Add(carrito);
                await _context.SaveChangesAsync();
                pedido.idCarrito = carrito.idCarrito;
            }
            // 2. Crear las líneas del carrito si no existen
            var lineasExistentes = await _context.CarritosLineas.Where(l => l.idCarrito == pedido.idCarrito).ToListAsync();
            if (lineasExistentes.Count == 0 && productosPedido != null)
            {
                foreach (var prod in productosPedido)
                {
                    var nuevaLinea = new CarritoLinea
                    {
                        idCarrito = pedido.idCarrito,
                        idProducto = prod.idProducto,
                        cantidad = prod.cantidad
                    };
                    _context.CarritosLineas.Add(nuevaLinea);
                }
                await _context.SaveChangesAsync();
            }
            // 3. Crear el pedido
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

            // Restar stock de los productos comprados
            foreach (var linea in lineas)
            {
                var productoDb = await _context.Productos.FirstOrDefaultAsync(p => p.idProducto == linea.idProducto);
                if (productoDb != null)
                {
                    productoDb.cantidad -= linea.cantidad;
                    if (productoDb.cantidad < 0)
                        productoDb.cantidad = 0;
                    _context.Productos.Update(productoDb);
                }
            }
            await _context.SaveChangesAsync();

            // Enviar email de confirmación de pedido
            var usuarioId = pedido.Carrito?.Id ?? carrito.Id;
            if (!string.IsNullOrEmpty(usuarioId))
            {
                var usuario = await _userManager.FindByIdAsync(usuarioId);
                if (usuario != null && !string.IsNullOrEmpty(usuario.Email))
                {
                    string subject = "Confirmación de tu pedido - No-Vacancy";
                    string body = $"<h2>¡Gracias por tu compra, {usuario.Nombre}!</h2><p>Tu pedido #{pedido.idPedido} ha sido registrado exitosamente.</p><p>Monto total: <strong>{detalle.monto:C}</strong></p>";
                    await _emailService.SendEmailAsync(usuario.Email, subject, body);
                }
            }

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

        // Acción MVC para mostrar la confirmación de pedido
        [HttpGet]
        public IActionResult PedidoConfirmado()
        {
            return View();
        }

        // Acción MVC para mostrar el historial de pedidos
        [HttpGet]
        public IActionResult Historial()
        {
            var pedidos = _context.Pedidos
                .Include(p => p.Carrito)
                .ToList();

            // Cargar usuarios de los carritos manualmente para evitar problemas de referencia nula
            var carritos = pedidos.Select(p => p.Carrito).Where(c => c != null && c.Id != null).ToList();
            var usuarioIds = carritos.Select(c => c.Id!).Distinct().ToList();
            var usuarios = _context.Set<Usuario>().Where(u => usuarioIds.Contains(u.Id)).ToList();
            foreach (var carrito in carritos)
            {
                carrito.Usuario = usuarios.FirstOrDefault(u => u.Id == carrito.Id);
            }

            // Obtener todas las líneas de los carritos involucrados
            var carritoIds = pedidos.Select(p => p.idCarrito).ToList();
            var lineas = _context.CarritosLineas
                .Where(l => carritoIds.Contains(l.idCarrito))
                .Include(l => l.Producto)
                .ThenInclude(p => p.Color)
                .Include(l => l.Producto)
                .ThenInclude(p => p.Talla)
                .ToList();
            var lineasPorCarrito = lineas.GroupBy(l => l.idCarrito)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Obtener detalles de los pedidos
            var pedidoIds = pedidos.Select(p => p.idPedido).ToList();
            var detalles = _context.Set<Detalle>()
                .Where(d => pedidoIds.Contains(d.idPedido))
                .ToList();
            var detallesPorPedido = detalles.ToDictionary(d => d.idPedido, d => d);

            ViewBag.LineasPorCarrito = lineasPorCarrito;
            ViewBag.DetallesPorPedido = detallesPorPedido;
            return View("Historial", pedidos);
        }
    }
}
