using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace NoVacancy.Controllers
{
    [Authorize] // Permite acceso a cualquier usuario autenticado
    public class ProductoController : Controller
    {
        private readonly NoVacancyDbContext _context;

        public ProductoController(NoVacancyDbContext context)
        {
            _context = context;
        }

        // GET: ProductoController
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .ToListAsync();
            // Retorna los productos como JSON para pruebas
            return Json(productos);
        }

        // GET: ProductoController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Color)
                .Include(p => p.Talla)
                .FirstOrDefaultAsync(p => p.idProducto == id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        // GET: ProductoController/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Tallas = _context.Tallas.ToList();
            ViewBag.Colores = _context.Colores.ToList();
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            string Titulo,
            string Descripcion,
            int CategoriaId,
            List<string> idColor, // Cambiado de List<int> a List<string>
            List<string> nuevoColor,
            List<string> nuevoCodigo,
            List<int> idTalla,
            List<decimal> Precio,
            List<int> cantidad,
            List<int?> limite
        )
        {
            // Validación de datos recibidos
            if (idColor == null || idTalla == null || Precio == null || cantidad == null || limite == null)
            {
                return BadRequest("Faltan datos requeridos para crear productos.");
            }
            System.Diagnostics.Debug.WriteLine($"idColor: {idColor?.Count ?? 0}, idTalla: {idTalla?.Count ?? 0}, Precio: {Precio?.Count ?? 0}, cantidad: {cantidad?.Count ?? 0}, limite: {limite?.Count ?? 0}");
            if (idColor != null && idColor.Count > 0) System.Diagnostics.Debug.WriteLine($"idColor[0]: {idColor[0]}");
            if (idTalla != null && idTalla.Count > 0) System.Diagnostics.Debug.WriteLine($"idTalla[0]: {idTalla[0]}");
            if (Precio != null && Precio.Count > 0) System.Diagnostics.Debug.WriteLine($"Precio[0]: {Precio[0]}");
            if (cantidad != null && cantidad.Count > 0) System.Diagnostics.Debug.WriteLine($"cantidad[0]: {cantidad[0]}");
            if (limite != null && limite.Count > 0) System.Diagnostics.Debug.WriteLine($"limite[0]: {limite[0]}");
            var files = Request.Form.Files;
            var productosInsertados = new List<Producto>();
            int variantes = new[] { idColor?.Count ?? 0, idTalla?.Count ?? 0, Precio?.Count ?? 0, cantidad?.Count ?? 0, limite?.Count ?? 0 }.Max();
            for (int i = 0; i < variantes; i++)
            {
                if ((idColor == null || i >= idColor.Count) ||
                    (idTalla == null || i >= idTalla.Count) ||
                    (Precio == null || i >= Precio.Count) ||
                    (cantidad == null || i >= cantidad.Count) ||
                    (limite == null || i >= limite.Count))
                {
                    continue;
                }
                int colorId = 0;
                if (idColor[i] == "nuevo" && nuevoColor != null && nuevoCodigo != null &&
                    nuevoColor.Count > i && nuevoCodigo.Count > i &&
                    !string.IsNullOrEmpty(nuevoColor[i]) && !string.IsNullOrEmpty(nuevoCodigo[i]))
                {
                    var color = new Color { nombre = nuevoColor[i], codigo = nuevoCodigo[i] };
                    _context.Colores.Add(color);
                    await _context.SaveChangesAsync();
                    colorId = color.idColor;
                }
                else if (int.TryParse(idColor[i], out int parsedColorId))
                {
                    colorId = parsedColorId;
                }
                else
                {
                    continue; // Si no es válido, saltar
                }
                var producto = new Producto
                {
                    nombre = Titulo,
                    descripcion = Descripcion,
                    idCategoria = CategoriaId,
                    idColor = colorId,
                    idTalla = idTalla[i],
                    precio = (double)Precio[i],
                    cantidad = cantidad[i],
                    limite = limite[i]
                };
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                productosInsertados.Add(producto);
                // Guardar imágenes asociadas a esta variante
                var imagenesVariante = new List<IFormFile>();
                foreach (var file in files)
                {
                    if (file.Name == $"Imagenes[{i}]")
                    {
                        imagenesVariante.Add(file);
                    }
                }
                foreach (var img in imagenesVariante)
                {
                    if (img != null && img.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/productos");
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);
                        var uniqueFileName = $"producto_{producto.idProducto}_{Guid.NewGuid()}_{Path.GetFileName(img.FileName)}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }
                        var imagen = new Imagen
                        {
                            nombre = uniqueFileName,
                            idProducto = producto.idProducto
                        };
                        _context.Imagenes.Add(imagen);
                    }
                }
            }
            await _context.SaveChangesAsync();
            if (productosInsertados.Count == 0)
            {
                return BadRequest("No se insertó ningún producto. Verifica los datos enviados.");
            }
            // Devuelve los productos insertados como JSON para verificar la inserción
            return Json(productosInsertados);
        }

        // GET: ProductoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idProducto,nombre,cantidad,limite,descripcion,precio,idTalla,idCategoria,idColor")] Producto producto)
        {
            if (id != producto.idProducto)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Productos.Any(e => e.idProducto == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(producto);
        }

        // GET: ProductoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        // POST: ProductoController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}