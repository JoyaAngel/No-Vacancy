using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using NoVacancy.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace NoVacancy.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NoVacancyDbContext _context;

        public HomeController(ILogger<HomeController> logger, NoVacancyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Productos más comprados
            var productosMasComprados = await _context.CarritosLineas
                .GroupBy(cl => cl.idProducto)
                .Select(g => new {
                    idProducto = g.Key,
                    TotalComprado = g.Sum(x => x.cantidad)
                })
                .OrderByDescending(x => x.TotalComprado)
                .Take(3)
                .ToListAsync();

            var productosCompradosIds = productosMasComprados.Select(x => x.idProducto).ToList();
            var productosComprados = await _context.Productos
                .Where(p => productosCompradosIds.Contains(p.idProducto))
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .Include(p => p.Categoria)
                .ToListAsync();
            var imagenesComprados = await _context.Imagenes
                .Where(i => productosCompradosIds.Contains(i.idProducto))
                .ToListAsync();

            // Productos mejor calificados
            var productosMejorCalificados = await _context.Resenias
                .GroupBy(r => r.idProducto)
                .Select(g => new {
                    idProducto = g.Key,
                    Promedio = g.Average(x => x.calificacion)
                })
                .OrderByDescending(x => x.Promedio)
                .Take(3)
                .ToListAsync();
            var productosCalificadosIds = productosMejorCalificados.Select(x => x.idProducto).ToList();
            var productosCalificados = await _context.Productos
                .Where(p => productosCalificadosIds.Contains(p.idProducto))
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .Include(p => p.Categoria)
                .ToListAsync();
            var imagenesCalificados = await _context.Imagenes
                .Where(i => productosCalificadosIds.Contains(i.idProducto))
                .ToListAsync();

            // Productos más recientes (últimos 3 agregados)
            var productosRecientes = await _context.Productos
                .OrderByDescending(p => p.idProducto)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .Include(p => p.Categoria)
                .Take(3)
                .ToListAsync();
            var imagenesRecientes = await _context.Imagenes
                .Where(i => productosRecientes.Select(p => p.idProducto).Contains(i.idProducto))
                .ToListAsync();

            ViewBag.ProductosMasComprados = productosComprados.Select(p => new {
                Producto = p,
                Imagen = imagenesComprados.FirstOrDefault(i => i.idProducto == p.idProducto)?.nombre
            }).ToList();
            ViewBag.ProductosMejorCalificados = productosCalificados.Select(p => new {
                Producto = p,
                Imagen = imagenesCalificados.FirstOrDefault(i => i.idProducto == p.idProducto)?.nombre
            }).ToList();
            ViewBag.ProductosRecientes = productosRecientes.Select(p => new {
                Producto = p,
                Imagen = imagenesRecientes.FirstOrDefault(i => i.idProducto == p.idProducto)?.nombre
            }).ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Product_detail(string nombre)
        {
            // Obtener todas las variantes del producto por nombre
            var variantes = await _context.Productos
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .Include(p => p.Categoria)
                .Where(p => p.nombre == nombre)
                .ToListAsync();
            if (!variantes.Any())
                return NotFound();
            // Agrupar por color
            var colores = variantes
                .GroupBy(v => v.Color)
                .Select(g => new {
                    Color = g.Key,
                    Tallas = g.Select(x => x.Talla).Distinct().ToList(),
                    ProductoIds = g.Select(x => x.idProducto).ToList()
                }).ToList();
            // Obtener imágenes por productoId
            var imagenesPorColor = new Dictionary<int, List<Imagen>>();
            foreach (var grupo in colores)
            {
                if (grupo.Color != null)
                {
                    var imagenes = await _context.Imagenes
                        .Where(i => grupo.ProductoIds.Contains(i.idProducto))
                        .ToListAsync();
                    // Agrupa por nombre base (sin el prefijo producto_{idProducto}_{Guid}_)
                    var regex = new Regex(@"producto_\d+_[a-fA-F0-9\-]+_(.+)");
                    imagenesPorColor[grupo.Color.idColor] = imagenes
                        .GroupBy(img => {
                            var match = regex.Match(img.nombre ?? "");
                            return match.Success ? match.Groups[1].Value : (img.nombre ?? "");
                        })
                        .Select(g => g.First())
                        .ToList();
                }
            }
            var productoIds = variantes.Select(v => v.idProducto).ToList();
            var resenias = await _context.Resenias
                .Where(r => productoIds.Contains(r.idProducto))
                .Include(r => r.Producto)
                .ToListAsync();
            ViewBag.Resenias = resenias;
            ViewBag.Colores = colores;
            ViewBag.ImagenesPorColor = imagenesPorColor;
            ViewBag.Nombre = nombre;
            ViewBag.Descripcion = variantes.First().descripcion;
            ViewBag.Precio = variantes.First().precio;
            ViewBag.Categoria = variantes.First().Categoria?.nombre;
            ViewBag.Variantes = variantes.Select(v => new { v.idProducto, v.idColor, v.idTalla, v.cantidad, v.precio, v.limite }).ToList();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Store(string? search, string? categoria, string? color, string? talla, decimal? precioMin, decimal? precioMax, int pagina = 1)
        {
            int pageSize = 9; // Productos por página
            var productosQuery = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .AsQueryable();

            // Filtros
            if (!string.IsNullOrWhiteSpace(search))
                productosQuery = productosQuery.Where(p => p.nombre != null && p.nombre.Contains(search));
            if (!string.IsNullOrWhiteSpace(categoria))
                productosQuery = productosQuery.Where(p => p.Categoria != null && p.Categoria.nombre == categoria);
            if (!string.IsNullOrWhiteSpace(color))
                productosQuery = productosQuery.Where(p => p.Color != null && p.Color.nombre == color);
            if (!string.IsNullOrWhiteSpace(talla))
                productosQuery = productosQuery.Where(p => p.Talla != null && p.Talla.nombre == talla);
            if (precioMin.HasValue)
                productosQuery = productosQuery.Where(p => p.precio >= (double)precioMin.Value);
            if (precioMax.HasValue)
                productosQuery = productosQuery.Where(p => p.precio <= (double)precioMax.Value);

            // Agrupar por nombre y tomar solo el primero de cada grupo
            var productosUnicos = await productosQuery
                .ToListAsync();
            productosUnicos = productosUnicos
                .GroupBy(p => p.nombre)
                .Select(g => g.First())
                .ToList();

            // Paginación
            int totalProductos = productosUnicos.Count;
            int totalPaginas = (int)Math.Ceiling(totalProductos / (double)pageSize);
            pagina = Math.Max(1, Math.Min(pagina, totalPaginas == 0 ? 1 : totalPaginas));
            var productosPagina = productosUnicos
                .Skip((pagina - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var productosVM = new List<ProductoTiendaViewModel>();
            foreach (var producto in productosPagina)
            {
                // Buscar imágenes para el idProducto
                var imagenes = await _context.Imagenes.Where(i => i.idProducto == producto.idProducto).ToListAsync();
                // Si no hay imágenes, buscar imágenes de cualquier producto con el mismo nombre
                if (imagenes == null || imagenes.Count == 0)
                {
                    var idsMismoNombre = productosUnicos.Where(p => p.nombre == producto.nombre).Select(p => p.idProducto).ToList();
                    imagenes = await _context.Imagenes.Where(i => idsMismoNombre.Contains(i.idProducto)).ToListAsync();
                }
                productosVM.Add(new ProductoTiendaViewModel
                {
                    Producto = producto,
                    Imagenes = imagenes
                });
            }

            // Llenar filtros
            var categorias = await _context.Categorias.ToListAsync();
            var colores = await _context.Colores.ToListAsync();
            var tallas = await _context.Tallas.ToListAsync();

            var viewModel = new TiendaViewModel
            {
                Productos = productosVM,
                Categorias = categorias,
                Colores = colores,
                Tallas = tallas,
                TotalPaginas = totalPaginas,
                PaginaActual = pagina
            };
            return View(viewModel);
        }

        public IActionResult Shopping_cart()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
