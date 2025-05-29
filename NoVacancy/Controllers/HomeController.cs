using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using NoVacancy.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NoVacancyDbContext _context;

        public HomeController(ILogger<HomeController> logger, NoVacancyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
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
                    imagenesPorColor[grupo.Color.idColor] = imagenes;
                }
            }
            ViewBag.Colores = colores;
            ViewBag.ImagenesPorColor = imagenesPorColor;
            ViewBag.Nombre = nombre;
            ViewBag.Descripcion = variantes.First().descripcion;
            ViewBag.Precio = variantes.First().precio;
            ViewBag.Categoria = variantes.First().Categoria?.nombre;
            ViewBag.Variantes = variantes.Select(v => new { v.idColor, v.idTalla, v.cantidad, v.precio, v.limite }).ToList();
            return View();
        }

        public async Task<IActionResult> Store()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .ToListAsync();
            // Agrupar por nombre y tomar solo el primero de cada grupo
            var productosUnicos = productos
                .GroupBy(p => p.nombre)
                .Select(g => g.First())
                .ToList();
            var productosVM = new List<ProductoTiendaViewModel>();
            foreach (var producto in productosUnicos)
            {
                var imagenes = await _context.Imagenes.Where(i => i.idProducto == producto.idProducto).ToListAsync();
                productosVM.Add(new ProductoTiendaViewModel
                {
                    Producto = producto,
                    Imagenes = imagenes
                });
            }
            var viewModel = new TiendaViewModel { Productos = productosVM };
            return View(viewModel);
        }

        public IActionResult Shopping_cart()
        {
            return View();
        }

        // Eliminar o comentar los métodos Login y Register, ya que ahora se usan desde UsuarioController.
        // public IActionResult Register()
        // {
        //     return View();
        // }

        // public IActionResult Login()
        // {
        //     return View();
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
