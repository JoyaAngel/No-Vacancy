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
            var lineasInvalidas = new List<CarritoLinea>();
            foreach (var linea in lineas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == linea.idProducto);
                if (producto == null || producto.cantidad <= 0 || linea.cantidad > producto.cantidad)
                {
                    lineasInvalidas.Add(linea);
                }
            }
            if (lineasInvalidas.Count > 0)
            {
                _context.CarritosLineas.RemoveRange(lineasInvalidas);
                _context.SaveChanges();
                // Actualizar la lista de líneas después de eliminar
                lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito).ToList();
            }
            if (lineas.Count == 0)
            {
                // Crear el nuevo carrito y reinsertar productos no procesados
                var nuevoCarrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(nuevoCarrito);
                _context.SaveChanges();
                foreach (var lineaInvalida in lineasInvalidas)
                {
                    var producto = _context.Productos.FirstOrDefault(p => p.idProducto == lineaInvalida.idProducto);
                    if (producto != null && producto.cantidad > 0)
                    {
                        int cantidadAInsertar = Math.Min(producto.cantidad, lineaInvalida.cantidad);
                        if (cantidadAInsertar > 0)
                        {
                            var nuevaLinea = new CarritoLinea
                            {
                                idCarrito = nuevoCarrito.idCarrito,
                                idProducto = producto.idProducto,
                                cantidad = cantidadAInsertar
                            };
                            _context.CarritosLineas.Add(nuevaLinea);
                        }
                    }
                }
                _context.SaveChanges();
                TempData["Error"] = "No hay productos válidos para confirmar el pedido. Los productos sin stock han sido eliminados del carrito y los restantes se han transferido al nuevo carrito.";
                return RedirectToAction("Shopping_cart", "CarritoLinea");
            }

            double total = 0;
            foreach (var linea in lineas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == linea.idProducto);
                if (producto != null)
                {
                    total += (producto.precio * linea.cantidad);
                    // Restar stock
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

            // Crear el nuevo carrito y reinsertar productos no procesados
            var nuevoCarritoDespues = new CarritoCabecera { Id = usuarioId };
            _context.CarritosCabecera.Add(nuevoCarritoDespues);
            _context.SaveChanges();
            foreach (var lineaInvalida in lineasInvalidas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == lineaInvalida.idProducto);
                if (producto != null && producto.cantidad > 0)
                {
                    int cantidadAInsertar = Math.Min(producto.cantidad, lineaInvalida.cantidad);
                    if (cantidadAInsertar > 0)
                    {
                        var nuevaLinea = new CarritoLinea
                        {
                            idCarrito = nuevoCarritoDespues.idCarrito,
                            idProducto = producto.idProducto,
                            cantidad = cantidadAInsertar
                        };
                        _context.CarritosLineas.Add(nuevaLinea);
                    }
                }
            }
            _context.SaveChanges();

            TempData["PedidoId"] = pedido.idPedido;
            return RedirectToAction("PedidoConfirmado", "Pedido");
        }
    }
}
