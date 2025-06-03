using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NoVacancy.ViewModels;
using Microsoft.AspNetCore.Authorization;
using NoVacancy.Services; // Ensure this using directive is present

namespace NoVacancy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly NoVacancyDbContext _context;

        // Servicios de identity
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailService _emailService;

        public UsuarioController(NoVacancyDbContext context, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager, IEmailService emailService) // MODIFICACIÓN 2: Cambiado de EmailService a IEmailService en el constructor
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        // GET: UsuarioController
        [AllowAnonymous]
        public IActionResult Index()
        {
            var usuarios = _userManager.Users.ToList();
            return View(usuarios);
        }


        // GET: UsuarioController/Details/{id}
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // GET: UsuarioController/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegistroUsuarioViewModel { Rol = "Cliente" };
            return View(model);
        }


        // POST: UsuarioController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistroUsuarioViewModel model)
        {
            System.Diagnostics.Debug.WriteLine("[DEBUG] Entró al método POST Register");
            // Forzar el rol a 'Cliente' siempre
            model.Rol = "Cliente";
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] ModelState válido");
                var existe = await _userManager.FindByEmailAsync(model.Email);
                if (existe != null)
                {
                    System.Diagnostics.Debug.WriteLine("[DEBUG] Correo ya registrado");
                    ModelState.AddModelError("Email", "El correo ya está registrado.");
                    return View(model);
                }

                var usuario = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Rol = model.Rol,
                    Calle = model.Calle,
                    Numero = model.Numero,
                    Colonia = model.Colonia,
                    Ciudad = model.Ciudad,
                    Estado = model.Estado,
                    CodigoPostal = model.CodigoPostal
                };

                var result = await _userManager.CreateAsync(usuario, model.Password);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.Rol))
                        await _userManager.AddToRoleAsync(usuario, model.Rol);

                    // Crear CarritoCabecera si el usuario es Cliente
                    if (model.Rol == "Cliente")
                    {
                        var carrito = new CarritoCabecera
                        {
                            Id = usuario.Id
                        };
                        _context.CarritosCabecera.Add(carrito);
                        await _context.SaveChangesAsync();
                    }

                    // Enviar correo de bienvenida
                    await _emailService.SendEmailAsync(model.Email, "Bienvenido a No-Vacancy", $"<h2>¡Hola {model.Nombre}!</h2><p>Tu registro fue exitoso.</p>");

                    // Iniciar sesión automáticamente
                    await _signInManager.SignInAsync(usuario, isPersistent: false);

                    // Redirección inteligente
                    var returnUrl = Request.Query["ReturnUrl"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(returnUrl))
                        return Redirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("[DEBUG] ModelState inválido");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null)
                    {
                        foreach (var error in errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"[ModelState] {key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            return View(model);
        }


        // GET: UsuarioController/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var usuario = await _userManager.FindByEmailAsync(model.Correo);
            if (usuario != null)
            {
                var result = await _signInManager.PasswordSignInAsync(usuario, model.Contrasenia, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View(model);
        }

        // GET: UsuarioController/Edit/{Id}
        public async Task<IActionResult> Edit(String id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,nombre,Email,rol")] Usuario usuarioEditado)
        {
            if (id != usuarioEditado.Id)
                return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                usuario.Nombre = usuarioEditado.Nombre;
                usuario.Email = usuarioEditado.Email;
                usuario.UserName = usuarioEditado.Email;
                usuario.Rol = usuarioEditado.Rol;

                var result = await _userManager.UpdateAsync(usuario);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(usuarioEditado);
        }


        // GET: UsuarioController/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                await _userManager.DeleteAsync(usuario);
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: UsuarioController/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: UsuarioController/Perfil
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> Perfil()
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login");

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            // Cargar historial de pedidos
            var pedidos = await _context.Pedidos
                .Include(p => p.Carrito)
                .Where(p => p.Carrito != null && p.Carrito.Id == usuarioId)
                .OrderByDescending(p => p.FechaHoraPedido)
                .ToListAsync();

            // Cargar detalles de cada pedido
            var pedidoIds = pedidos.Select(p => p.idPedido).ToList();
            var detalles = await _context.Set<Detalle>()
                .Where(d => pedidoIds.Contains(d.idPedido))
                .ToListAsync();
            foreach (var pedido in pedidos)
            {
                pedido.Detalle = detalles.FirstOrDefault(d => d.idPedido == pedido.idPedido);
            }

            ViewBag.Pedidos = pedidos;
            return View(usuario);
        }

        // GET: UsuarioController/EditarPerfil
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EditarPerfil()
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login");

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: UsuarioController/EditarPerfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EditarPerfil(Usuario usuarioEditado)
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null || usuarioId != usuarioEditado.Id)
                return Unauthorized();

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                usuario.Nombre = usuarioEditado.Nombre;
                usuario.Calle = usuarioEditado.Calle;
                usuario.Numero = usuarioEditado.Numero;
                usuario.Colonia = usuarioEditado.Colonia;
                usuario.Ciudad = usuarioEditado.Ciudad;
                usuario.Estado = usuarioEditado.Estado;
                usuario.CodigoPostal = usuarioEditado.CodigoPostal;
                // Mantener el rol original
                usuario.Rol = usuario.Rol;
                var result = await _userManager.UpdateAsync(usuario);
                if (result.Succeeded)
                    return RedirectToAction("Perfil");
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            } else {
                // Mostrar errores de validación
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key]?.Errors;
                    if (errors != null)
                    {
                        foreach (var error in errors)
                        {
                            System.Diagnostics.Debug.WriteLine($"[ModelState] {key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            return View(usuario);
        }

        // GET: UsuarioController/CambiarContrasena
        [Authorize(Roles = "Cliente")]
        public IActionResult CambiarContrasena()
        {
            return View();
        }

        // POST: UsuarioController/CambiarContrasena
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CambiarContrasena(string actual, string nueva, string confirmar)
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (usuarioId == null)
                return RedirectToAction("Login");

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound();

            if (nueva != confirmar)
            {
                ModelState.AddModelError("", "La nueva contraseña y la confirmación no coinciden.");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(usuario, actual, nueva);
            if (result.Succeeded)
                return RedirectToAction("Perfil");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View();
        }

        // GET: UsuarioController/DetallesPedido/{id}
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> DetallesPedido(int id)
        {
            var usuarioId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var pedido = await _context.Pedidos
                .Include(p => p.Carrito)
                .FirstOrDefaultAsync(p => p.idPedido == id && p.Carrito != null && p.Carrito.Id == usuarioId);
            if (pedido == null)
                return NotFound();

            var lineas = await _context.CarritosLineas
                .Where(l => l.idCarrito == pedido.idCarrito)
                .Include(l => l.Producto)
                .ThenInclude(p => p.Color)
                .Include(l => l.Producto)
                .ThenInclude(p => p.Talla)
                .ToListAsync();

            ViewBag.Lineas = lineas;
            var detalle = await _context.Set<Detalle>().FirstOrDefaultAsync(d => d.idPedido == pedido.idPedido);
            ViewBag.Detalle = detalle;

            // Obtener reseñas del usuario para los productos de este pedido
            var productoIds = lineas.Select(l => l.idProducto).ToList();
            var resenias = await _context.Resenias
                .Where(r => productoIds.Contains(r.idProducto) && r.idPedido == pedido.idPedido)
                .ToListAsync();
            var reseniasPorProducto = resenias
                .GroupBy(r => r.idProducto)
                .ToDictionary(g => g.Key, g => g.First());
            ViewBag.ReseniasPorProducto = reseniasPorProducto;

            return View(pedido);
        }

        // GET: UsuarioController/DebugIdentity
        [AllowAnonymous]
        public IActionResult DebugIdentity()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
            var name = User.Identity?.Name;
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            var roles = User.IsInRole("Cliente") ? "Cliente" : "(no Cliente)";
            return Content($"Authenticated: {isAuthenticated}\nName: {name}\nRoles: {roles}\nClaims:\n" + string.Join("\n", claims));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult OlvideContrasenia()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> OlvideContrasenia(RecuperarContraseniaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.FindByEmailAsync(model.Correo);
            if (usuario == null)
            {
                // No revelamos que el usuario no existe por seguridad
                TempData["Mensaje"] = "Si el correo está registrado, se enviará un enlace para restablecer la contraseña.";
                return RedirectToAction(nameof(OlvideContrasenia));
            }

            // Generar token de reseteo de contraseña
            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

            // Crear enlace de reseteo
            var callbackUrl = Url.Action(
            "RestablecerContrasenia",
            "Usuario",
            new { email = usuario.Email, token = token },
            protocol: HttpContext.Request.Scheme);


            // Enviar correo con el enlace
            var mensaje = $@"
            <h2>Recuperar contraseña</h2>
            <p>Haz clic en el siguiente enlace para restablecer tu contraseña:</p>
            <p><a href='{callbackUrl}'>Restablecer contraseña</a></p>";

            await _emailService.SendEmailAsync(model.Correo, "Restablecer contraseña - No-Vacancy", mensaje);

            TempData["Mensaje"] = "Si el correo está registrado, se enviará un enlace para restablecer la contraseña.";
            return RedirectToAction(nameof(OlvideContrasenia));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult RestablecerContrasenia(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest("Faltan datos para restablecer la contraseña.");

            var model = new RestablecerContraseniaViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestablecerContrasenia(RestablecerContraseniaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _userManager.FindByEmailAsync(model.Email);
            if (usuario == null)
            {
                TempData["Mensaje"] = "La cuenta no existe.";
                return RedirectToAction("Login");
            }

            var resultado = await _userManager.ResetPasswordAsync(usuario, model.Token, model.NuevaContrasenia);
            if (resultado.Succeeded)
            {
                TempData["Mensaje"] = "Tu contraseña fue restablecida correctamente.";
                return RedirectToAction("Login");
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


    }
}