using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly NoVacancyDbContex _context;

        public UsuarioController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: UsuarioController
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // GET: UsuarioController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
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
        public async Task<IActionResult> Register([Bind("nombre,correo,constrasenia,rol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Usuarios.AnyAsync(u => u.correo == usuario.correo))
                {
                    ModelState.AddModelError("correo", "El correo ya está registrado.");
                    return View(usuario);
                }
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        // GET: UsuarioController/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: UsuarioController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string constrasenia)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.correo == correo && u.constrasenia == constrasenia);
            if (usuario != null)
            {
                // Aquí puedes guardar la sesión del usuario
                HttpContext.Session.SetInt32("UsuarioId", usuario.idUsuario);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View();
        }

        // GET: UsuarioController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        // POST: UsuarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idUsuario,nombre,correo,constrasenia")] Usuario usuario)
        {
            if (id != usuario.idUsuario)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Usuarios.Any(e => e.idUsuario == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(usuario);
        }

        // GET: UsuarioController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();
            return View(usuario);
        }

        // POST: UsuarioController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
