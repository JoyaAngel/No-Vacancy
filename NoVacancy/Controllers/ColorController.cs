using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NoVacancy.Controllers
{
    public class ColorController : Controller
    {
        // GET: ColorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ColorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ColorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ColorController/Create
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

        // GET: ColorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ColorController/Edit/5
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

        // GET: ColorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ColorController/Delete/5
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
