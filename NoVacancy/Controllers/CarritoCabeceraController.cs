using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using NoVacancy.Services;
using System.Security.Claims;

namespace NoVacancy.Controllers
{
    public class CarritoCabeceraController : Controller
    {
        readonly NoVacancyDbContext _context;
        readonly IEmailService _emailService;

        public CarritoCabeceraController(NoVacancyDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
        public async Task<IActionResult> ConfirmarPedido(
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
            string? usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? correoUsuario = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(usuarioId))
                return RedirectToAction("Login", "Usuario");

            if (solicitarFactura && (string.IsNullOrWhiteSpace(rfc) || string.IsNullOrWhiteSpace(regimenFiscal) || string.IsNullOrWhiteSpace(codigoPostal)))
            {
                TempData["Error"] = "Para solicitar factura debes llenar RFC, Régimen fiscal y Código postal.";
                return RedirectToAction("Shopping_cart", "CarritoLinea");
            }

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
            var lineasInvalidas = new List<(CarritoLinea linea, int cantidadNoProcesada)>();

            foreach (var linea in lineas)
            {
                var producto = _context.Productos.FirstOrDefault(p => p.idProducto == linea.idProducto);
                if (producto == null || producto.cantidad <= 0)
                {
                    lineasInvalidas.Add((linea, linea.cantidad));
                }
                else if (linea.cantidad > producto.cantidad)
                {
                    int cantidadProcesable = producto.cantidad;
                    int cantidadNoProcesada = linea.cantidad - cantidadProcesable;
                    if (cantidadNoProcesada > 0)
                        lineasInvalidas.Add((linea, cantidadNoProcesada));
                    linea.cantidad = cantidadProcesable;
                }
            }

            if (lineasInvalidas.Count > 0)
            {
                var nuevoCarrito = new CarritoCabecera { Id = usuarioId };
                _context.CarritosCabecera.Add(nuevoCarrito);
                await _context.SaveChangesAsync();

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
                    _context.CarritosLineas.Remove(lineaInvalida);
                }

                await _context.SaveChangesAsync();

                lineas = _context.CarritosLineas.Where(l => l.idCarrito == carrito.idCarrito).ToList();
                if (lineas.Count == 0)
                {
                    TempData["Error"] = "No hay productos válidos para confirmar el pedido. Los productos sin stock han sido eliminados del carrito y los restantes se han transferido al nuevo carrito.";
                    return RedirectToAction("Shopping_cart", "CarritoLinea");
                }
            }

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
            await _context.SaveChangesAsync();

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
            await _context.SaveChangesAsync();

            var detalle = new Detalle
            {
                monto = total,
                idPedido = pedido.idPedido
            };
            _context.Set<Detalle>().Add(detalle);
            await _context.SaveChangesAsync();

            if (lineasInvalidas.Count == 0)
            {
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
                    await _context.SaveChangesAsync();
                }
            }

            // ✅ Envío de correo
            if (!string.IsNullOrEmpty(correoUsuario))
            {
                string asunto = "Confirmación de tu pedido en NoVacancy";
                string mensajeHtml = $@"
                    <h2>¡Gracias por tu compra!</h2>
                    <p>Tu pedido con ID <strong>{pedido.idPedido}</strong> fue confirmado correctamente el {pedido.FechaHoraPedido}.</p>
                    <p><strong>Total:</strong> ${total:N2}</p>
                    <p><strong>Enviado a:</strong> {Calle} {Numero}, {Colonia}, {Ciudad}, {Estado}, CP: {CodigoPostalEnvio}</p>";

                await _emailService.SendEmailAsync(correoUsuario, asunto, mensajeHtml);
            }

            TempData["PedidoId"] = pedido.idPedido;
            return RedirectToAction("PedidoConfirmado", "Pedido");
        }
    }
}
