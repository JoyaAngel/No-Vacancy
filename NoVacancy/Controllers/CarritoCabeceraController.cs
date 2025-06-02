using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;

namespace NoVacancy.Controllers
{
    public class CarritoCabeceraController : Controller
    {
        readonly NoVacancyDbContext _context;
        public CarritoCabeceraController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: CarritosCabecera
        public IActionResult GetCarritosCabecera()
        {
            var carritosCabecera = _context.CarritosCabecera.ToList();
            return Ok(carritosCabecera);
        }

        // POST: CarritosCabecera
        [HttpPost]
        public IActionResult Create([FromBody] CarritoCabecera carritoCabecera)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            _context.CarritosCabecera.Add(carritoCabecera);
            _context.SaveChanges();
            return Ok(carritoCabecera);
        }

        [HttpPost]
        public IActionResult ConfirmarPedido(
            [FromForm] bool solicitarFactura,
            [FromForm] string? rfc,
            [FromForm] string? regimenFiscal,
            [FromForm] string? codigoPostal,
            [FromForm] string Calle,
            [FromForm] string Numero,
            [FromForm] string Colonia,
            [FromForm] string Ciudad,
            [FromForm] string Estado,
            [FromForm] string CodigoPostalEnvio)
        {
            string? usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Usuario");

            // Validación de campos de factura
            if (solicitarFactura && (string.IsNullOrWhiteSpace(rfc) || string.IsNullOrWhiteSpace(regimenFiscal) || string.IsNullOrWhiteSpace(codigoPostal)))
            {
                TempData["Error"] = "Para solicitar factura debes llenar RFC, Régimen fiscal y Código postal.";
                return RedirectToAction("Shopping_cart", "CarritoLinea");
            }

            // Buscar el carrito activo del usuario (el más reciente sin pedido asociado)
            var carrito = _context.CarritosCabecera
                .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                .OrderByDescending(c => c.idCarrito)
                .FirstOrDefault();
            if (carrito == null)
            {
                TempData["Error"] = "No se encontró un carrito para este usuario.";
                return RedirectToAction("Shopping_cart", "CarritoLinea");
            }

            var lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito).ToList();
            // Guardar productos inválidos para reinsertar en el nuevo carrito
            var lineasInvalidas = new List<(CarritoLinea linea, int cantidadNoProcesada)>();
            foreach (var linea in lineas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == linea.idProducto);
                if (producto == null || producto.cantidad <= 0)
                {
                    // No hay stock, transfiere toda la cantidad
                    lineasInvalidas.Add((linea, linea.cantidad));
                }
                else if (linea.cantidad > producto.cantidad)
                {
                    // Hay stock parcial, procesa lo posible y transfiere el resto
                    int cantidadProcesable = producto.cantidad;
                    int cantidadNoProcesada = linea.cantidad - cantidadProcesable;
                    if (cantidadNoProcesada > 0)
                        lineasInvalidas.Add((linea, cantidadNoProcesada));
                    // Ajusta la línea para que solo procese la cantidad posible
                    linea.cantidad = cantidadProcesable;
                }
            }
            // 1. Si hay productos inválidos, crear el nuevo carrito y transferirlos
            if (lineasInvalidas.Count > 0)
            {
                var nuevoCarrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(nuevoCarrito);
                _context.SaveChanges();
                foreach (var (lineaInvalida, cantidadNoProcesada) in lineasInvalidas)
                {
                    var producto = _context.Productos.FirstOrDefault(p => p.idProducto == lineaInvalida.idProducto);
                    if (producto != null && cantidadNoProcesada > 0)
                    {
                        var nuevaLinea = new CarritoLinea
                        {
                            idCarrito = nuevoCarrito.idCarrito,
                            idProducto = producto.idProducto,
                            cantidad = cantidadNoProcesada
                        };
                        _context.CarritosLineas.Add(nuevaLinea);
                    }
                    // Eliminar la línea inválida del carrito original
                    _context.CarritosLineas.Remove(lineaInvalida);
                }
                _context.SaveChanges();
                // Actualizar la lista de líneas después de eliminar las inválidas
                lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito).ToList();
                if (lineas.Count == 0)
                {
                    TempData["Error"] = "No hay productos válidos para confirmar el pedido. Los productos sin stock han sido eliminados del carrito y los restantes se han transferido al nuevo carrito.";
                    return RedirectToAction("Shopping_cart", "CarritoLinea");
                }
            }

            // 2. Procesar los productos válidos (restar stock, crear pedido, detalle, etc.)
            double total = 0;
            foreach (var linea in lineas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == linea.idProducto);
                if (producto != null)
                {
                    total += (producto.precio * linea.cantidad);
                    producto.cantidad -= linea.cantidad;
                    if (producto.cantidad < 0)
                        producto.cantidad = 0;
                    _context.Productos.Update(producto);
                }
            }
            _context.SaveChanges();

            var pedido = new Pedido
            {
                idCarrito = carrito.idCarrito,
                Calle = Calle,
                Numero = Numero,
                Colonia = Colonia,
                Ciudad = Ciudad,
                Estado = Estado,
                CodigoPostalEnvio = CodigoPostalEnvio,
                FechaHoraPedido = DateTime.Now,
                rfc = solicitarFactura ? rfc : null,
                regimen = solicitarFactura ? regimenFiscal : null,
                codigoPostal = solicitarFactura ? codigoPostal : null
            };
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            var detalle = new Detalle
            {
                monto = total,
                idPedido = pedido.idPedido
            };
            _context.Set<Detalle>().Add(detalle);
            _context.SaveChanges();

            // 3. Ya NO eliminar las líneas procesadas del carrito original (deja las líneas válidas para el historial)

            // 4. Crear el nuevo carrito vacío para el usuario (si no se creó antes)
            if (lineasInvalidas.Count == 0)
            {
                // Solo crear un nuevo carrito si el usuario NO tiene ya un carrito sin líneas y sin pedido asociado
                var existeCarritoVacio = _context.CarritosCabecera
                    .Where(c => c.Id == usuarioId && !_context.Pedidos.Any(p => p.idCarrito == c.idCarrito))
                    .OrderByDescending(c => c.idCarrito)
                    .Select(c => c.idCarrito)
                    .ToList()
                    .Any(idCarrito => !_context.CarritosLineas.Any(l => l.idCarrito == idCarrito));
                if (!existeCarritoVacio)
                {
                    var nuevoCarrito = new CarritoCabecera { Id = usuarioId };
                    _context.CarritosCabecera.Add(nuevoCarrito);
                    _context.SaveChanges();
                }
            }

            TempData["PedidoId"] = pedido.idPedido;
            return RedirectToAction("PedidoConfirmado", "Pedido");
        }
    }
}
