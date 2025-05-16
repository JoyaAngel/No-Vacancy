using Microsoft.AspNetCore.Mvc;
using NoVacancy.Models;
using NoVacancy.Data;
using Microsoft.EntityFrameworkCore;

namespace NoVacancy.Controllers
{
    public class ClienteController : Controller
    {
        private readonly NoVacancyDbContex _context;

        public ClienteController(NoVacancyDbContex context)
        {
            _context = context;
        }

        // GET: ClienteController
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.ToListAsync();
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // GET: ClienteController/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: ClienteController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("nombre,correo,constrasenia")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Clientes.AnyAsync(c => c.correo == cliente.correo))
                {
                    ModelState.AddModelError("correo", "El correo ya está registrado.");
                    return View(cliente);
                }
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                // Aquí podrías iniciar sesión automáticamente si lo deseas
                return RedirectToAction("Login");
            }
            return View(cliente);
        }

        // GET: ClienteController/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: ClienteController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string constrasenia)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.correo == correo && c.constrasenia == constrasenia);
            if (cliente != null)
            {
                // Aquí puedes guardar la sesión del usuario
                HttpContext.Session.SetInt32("ClienteId", cliente.idCliente);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View();
        }

        // GET: ClienteController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("idCliente,nombre,correo,constrasenia")] Cliente cliente)
        {
            if (id != cliente.idCliente)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Clientes.Any(e => e.idCliente == id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return View(cliente);
        }

        // GET: ClienteController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
