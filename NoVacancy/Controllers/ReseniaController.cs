using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NoVacancy.Controllers
{
    public class ReseniaController : Controller
    {
        // GET: ReseniaController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReseniaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReseniaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReseniaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReseniaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReseniaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReseniaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReseniaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
