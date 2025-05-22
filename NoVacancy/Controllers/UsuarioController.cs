using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NoVacancy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly NoVacancyDbContex _context;

        //Servicios de identity
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;

        public UsuarioController(NoVacancyDbContex context, SignInManager<Usuario> signInManager, UserManager<Usuario> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            var usuarios = _userManager.Users.ToList(); // No necesita await
            return View(usuarios);
        }


        // GET: UsuarioController/Details/{id}
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
        public IActionResult Register()
        {
            return View();
        }

        // POST: UsuarioController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("nombre,Email,constrasenia,rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var existe = await _userManager.FindByEmailAsync(usuario.Email);
                if (existe != null)
                {
                    ModelState.AddModelError("Email", "El correo ya está registrado.");
                    return View(usuario);
                }

                // Crear usuario con Identity
                var result = await _userManager.CreateAsync(usuario, usuario.PasswordHash);

                if (result.Succeeded)
                {
                    // Asignar rol si lo tiene
                    if (!string.IsNullOrEmpty(usuario.Rol))
                        await _userManager.AddToRoleAsync(usuario, usuario.Rol);

                    return RedirectToAction(nameof(Index));
                }

                // Errores de validación de Identity
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(usuario);
        }


        // GET: UsuarioController/Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
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
    }
}
