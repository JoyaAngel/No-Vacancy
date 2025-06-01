using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NoVacancy.ViewModels;
using Microsoft.AspNetCore.Authorization; //Separar las vistas de los modelos.


namespace NoVacancy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly NoVacancyDbContext _context;

        //Servicios de identity
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioController(NoVacancyDbContext context, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: UsuarioController
        [AllowAnonymous]
        public async Task<IActionResult> Index()
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
            return View();
        }


        // POST: UsuarioController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegistroUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existe = await _userManager.FindByEmailAsync(model.Email);
                if (existe != null)
                {
                    ModelState.AddModelError("Email", "El correo ya está registrado.");
                    return View(model);
                }

                var usuario = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Rol = model.Rol
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
        public async Task<IActionResult> Login(string correo, string constrasenia)
        {
            var usuario = await _userManager.FindByEmailAsync(correo);
            if (usuario != null)
            {
                var result = await _signInManager.PasswordSignInAsync(usuario, constrasenia, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
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
    }
}
