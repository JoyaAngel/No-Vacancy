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
            //return Json(productos);
            return View(productos);
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
        public async Task<IActionResult> Create([FromForm] NoVacancy.ViewModels.ProductoCreateViewModel model)
        {
            try
            {
                // DEBUG: Loguear el modelo recibido
                System.Diagnostics.Debug.WriteLine($"Titulo: {model.Titulo}");
                System.Diagnostics.Debug.WriteLine($"Descripcion: {model.Descripcion}");
                System.Diagnostics.Debug.WriteLine($"CategoriaId: {model.CategoriaId}");
                System.Diagnostics.Debug.WriteLine($"Variantes count: {model.Variantes?.Count ?? 0}");
                if (model.Variantes != null)
                {
                    for (int i = 0; i < model.Variantes.Count; i++)
                    {
                        var v = model.Variantes[i];
                        System.Diagnostics.Debug.WriteLine($"Variante[{i}]: IdColor={v.IdColor}, NuevoColor={v.NuevoColor}, NuevoCodigo={v.NuevoCodigo}, Precio={v.Precio}, Tallas={v.Tallas?.Count ?? 0}, Imagenes={v.Imagenes?.Count ?? 0}");
                        if (v.Tallas != null)
                        {
                            for (int j = 0; j < v.Tallas.Count; j++)
                            {
                                var t = v.Tallas[j];
                                System.Diagnostics.Debug.WriteLine($"  Talla[{j}]: IdTalla={t.IdTalla}, Cantidad={t.Cantidad}, Limite={t.Limite}");
                            }
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("[DEBUG] Antes de validar ModelState.IsValid");
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"[ModelState] {key}: {error.ErrorMessage}");
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("[DEBUG] ModelState inválido, retornando View(model)");
                    ViewBag.Categorias = _context.Categorias.ToList();
                    ViewBag.Tallas = _context.Tallas.ToList();
                    ViewBag.Colores = _context.Colores.ToList();
                    return View(model);
                }
                System.Diagnostics.Debug.WriteLine("[DEBUG] ModelState válido, entrando a ciclo de variantes");
                var productosInsertados = new List<Producto>();

                if (model.Variantes == null || model.Variantes.Count == 0)
                {
                    ModelState.AddModelError("Variantes", "Debes agregar al menos una variante.");
                    ViewBag.Categorias = _context.Categorias.ToList();
                    ViewBag.Tallas = _context.Tallas.ToList();
                    ViewBag.Colores = _context.Colores.ToList();
                    return View(model);
                }

                // VALIDACIÓN Y UNIFICACIÓN DE VARIANTES Y TALLAS
                if (model.Variantes != null)
                {
                    // Unificar variantes por color (IdColor o combinación de NuevoColor+NuevoCodigo)
                    var variantesUnificadas = new List<NoVacancy.ViewModels.VarianteViewModel>();
                    foreach (var variante in model.Variantes)
                    {
                        // Validar precio no negativo
                        if (variante.Precio < 0)
                        {
                            ModelState.AddModelError($"Variantes", $"El precio no puede ser negativo.");
                            break;
                        }
                        // Unificar tallas repetidas dentro de la variante
                        if (variante.Tallas != null)
                        {
                            var tallasUnificadas = variante.Tallas
                                .GroupBy(t => t.IdTalla)
                                .Select(g => new NoVacancy.ViewModels.TallaViewModel
                                {
                                    IdTalla = g.Key,
                                    Cantidad = g.Sum(x => x.Cantidad < 0 ? 0 : x.Cantidad),
                                    Limite = g.Sum(x => x.Limite.HasValue && x.Limite.Value > 0 ? x.Limite.Value : 0)
                                })
                                .Where(t => t.IdTalla > 0)
                                .ToList();
                            variante.Tallas = tallasUnificadas;
                        }
                        // Buscar si ya existe una variante con el mismo color
                        bool esNuevoColor = (variante.IdColor == null || variante.IdColor == 0);
                        string claveColor = esNuevoColor ? $"nuevo:{variante.NuevoColor}|{variante.NuevoCodigo}" : $"id:{variante.IdColor}";
                        var existente = variantesUnificadas.FirstOrDefault(v =>
                            ((v.IdColor == variante.IdColor && !esNuevoColor) ||
                            (v.IdColor == null && v.NuevoColor == variante.NuevoColor && v.NuevoCodigo == variante.NuevoCodigo))
                        );
                        if (existente != null)
                        {
                            if (variante.Tallas != null)
                            {
                                foreach (var talla in variante.Tallas)
                                {
                                    var tallaExistente = existente.Tallas.FirstOrDefault(t => t.IdTalla == talla.IdTalla);
                                    if (tallaExistente != null)
                                    {
                                        tallaExistente.Cantidad += talla.Cantidad;
                                        tallaExistente.Limite = (tallaExistente.Limite ?? 0) + (talla.Limite ?? 0);
                                    }
                                    else
                                    {
                                        existente.Tallas.Add(talla);
                                    }
                                }
                            }
                            // Unificar imágenes (opcional: solo mantener una lista sin duplicados por nombre)
                            if (variante.Imagenes != null)
                            {
                                if (existente.Imagenes == null) existente.Imagenes = new List<IFormFile>();
                                foreach (var img in variante.Imagenes)
                                {
                                    if (!existente.Imagenes.Any(e => e.FileName == img.FileName))
                                        existente.Imagenes.Add(img);
                                }
                            }
                        }
                        else
                        {
                            variantesUnificadas.Add(variante);
                        }
                    }
                    model.Variantes = variantesUnificadas;
                }
                // Validar que no haya valores negativos en tallas
                if (model.Variantes != null)
                {
                    foreach (var variante in model.Variantes)
                    {
                        if (variante.Precio < 0)
                        {
                            ModelState.AddModelError($"Variantes", $"El precio no puede ser negativo.");
                        }
                        if (variante.Tallas != null)
                        {
                            foreach (var talla in variante.Tallas)
                            {
                                if (talla.Cantidad < 0)
                                    ModelState.AddModelError($"Variantes", $"La cantidad no puede ser negativa.");
                                if (talla.Limite.HasValue && talla.Limite.Value < 0)
                                    ModelState.AddModelError($"Variantes", $"El límite no puede ser negativo.");
                            }
                        }
                    }
                }

                for (int i = 0; model.Variantes != null && i < model.Variantes.Count; i++)
                {
                    var variante = model.Variantes[i];
                    int colorId = 0;
                    // Si es un color nuevo
                    if ((variante.IdColor == null || variante.IdColor == 0) && !string.IsNullOrWhiteSpace(variante.NuevoColor) && !string.IsNullOrWhiteSpace(variante.NuevoCodigo))
                    {
                        var color = new Color { nombre = variante.NuevoColor, codigo = variante.NuevoCodigo };
                        _context.Colores.Add(color);
                        await _context.SaveChangesAsync();
                        colorId = color.idColor;
                    }
                    else if (variante.IdColor != null && variante.IdColor > 0)
                    {
                        colorId = variante.IdColor.Value;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Variante {i} con color inválido: IdColor={variante.IdColor}, NuevoColor='{variante.NuevoColor}', NuevoCodigo='{variante.NuevoCodigo}'");
                        continue; // Color inválido
                    }

                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Entrando a ciclo de tallas para variante {i}, total tallas: {variante.Tallas?.Count ?? 0}");
                    if (variante.Tallas == null || variante.Tallas.Count == 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Variante {i} sin tallas, se omite");
                        continue;
                    }
                    bool tallaAgregada = false;
                    foreach (var talla in variante.Tallas)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Analizando talla: IdTalla={talla.IdTalla}, Cantidad={talla.Cantidad}, Limite={talla.Limite}");
                        if (talla.IdTalla == 0 || talla.Cantidad < 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"[DEBUG] Talla omitida por condición: IdTalla={talla.IdTalla}, Cantidad={talla.Cantidad}");
                            continue;
                        }
                        tallaAgregada = true;
                        var producto = new Producto
                        {
                            nombre = model.Titulo,
                            descripcion = model.Descripcion,
                            idCategoria = model.CategoriaId,
                            idColor = colorId,
                            idTalla = talla.IdTalla,
                            precio = (double)variante.Precio,
                            cantidad = talla.Cantidad,
                            limite = talla.Limite
                        };
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] Agregando producto: nombre={producto.nombre}, idColor={producto.idColor}, idTalla={producto.idTalla}, precio={producto.precio}, cantidad={producto.cantidad}, limite={producto.limite}");
                        _context.Productos.Add(producto);
                        var affected = await _context.SaveChangesAsync();
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] SaveChangesAsync() productos afectadas: {affected}");
                        productosInsertados.Add(producto);

                        // Guardar imágenes asociadas a la variante
                        if (variante.Imagenes != null)
                        {
                            foreach (var img in variante.Imagenes)
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
                                    System.Diagnostics.Debug.WriteLine($"[DEBUG] Agregando imagen: nombre={imagen.nombre}, idProducto={imagen.idProducto}");
                                    _context.Imagenes.Add(imagen);
                                }
                            }
                        }
                    }
                    if (!tallaAgregada)
                    {
                        System.Diagnostics.Debug.WriteLine($"[DEBUG] No se agregó ninguna talla para variante {i}");
                    }
                }
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("[DEBUG] SaveChangesAsync() final ejecutado");
                if (productosInsertados.Count == 0)
                {
                    ModelState.AddModelError("", "No se insertó ningún producto. Verifica los datos enviados.");
                    ViewBag.Categorias = _context.Categorias.ToList();
                    ViewBag.Tallas = _context.Tallas.ToList();
                    ViewBag.Colores = _context.Colores.ToList();
                    return View(model);
                }                return RedirectToAction("Store", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar el producto: {ex.Message}");
                ViewBag.Categorias = _context.Categorias.ToList();
                ViewBag.Tallas = _context.Tallas.ToList();
                ViewBag.Colores = _context.Colores.ToList();
                return View(model);
            }
        }

        // GET: ProductoController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            // Cargar las listas necesarias para los dropdowns
            ViewBag.Categorias = await _context.Categorias.ToListAsync();
            ViewBag.Tallas = await _context.Tallas.ToListAsync();
            ViewBag.Colores = await _context.Colores.ToListAsync();

            return View(producto);
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.idProducto)
                return NotFound();

            // Validación de valores negativos
            if (producto.precio < 0)
                ModelState.AddModelError("precio", "El precio no puede ser negativo.");
            if (producto.cantidad < 0)
                ModelState.AddModelError("cantidad", "La cantidad no puede ser negativa.");
            if (producto.limite.HasValue && producto.limite.Value < 0)
                ModelState.AddModelError("limite", "El límite no puede ser negativo.");

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el producto existe
                    var productoExistente = await _context.Productos.FindAsync(id);
                    if (productoExistente == null)
                        return NotFound();

                    // Actualizar propiedades
                    productoExistente.nombre = producto.nombre;
                    productoExistente.descripcion = producto.descripcion;
                    productoExistente.precio = producto.precio;
                    productoExistente.cantidad = producto.cantidad;
                    productoExistente.limite = producto.limite;
                    productoExistente.idCategoria = producto.idCategoria;
                    productoExistente.idColor = producto.idColor;
                    productoExistente.idTalla = producto.idTalla;

                    _context.Update(productoExistente);
                    await _context.SaveChangesAsync();

                    return Redirect("/Admin/Index");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }

            // Si hay errores de validación, recargar los ViewBag
            ViewBag.Categorias = await _context.Categorias.ToListAsync();
            ViewBag.Tallas = await _context.Tallas.ToListAsync();
            ViewBag.Colores = await _context.Colores.ToListAsync();

            return View(producto);
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.idProducto == id);
        }

        // GET: ProductoController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return Redirect("/Admin/Index");
        }
    }
}