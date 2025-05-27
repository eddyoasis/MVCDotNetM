using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using System;
using System.Drawing.Printing;
using System.Linq;

namespace MVCWebApp.Controllers
{
    public class MarginControllerCopy : Controller
    {
        private List<MarginViewModel> _margins = new List<MarginViewModel>
            {
                new MarginViewModel {  ID = 1, Username = "dddddd", Percentage = 60},
                new MarginViewModel {  ID = 2, Username = "dddddd22", Percentage = 50}
            };

        public IActionResult Index()
        {
            return View(_margins);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            // Optionally, pass an empty model to the view if needed
            return View(new MarginViewModel());
        }

        // POST: Home/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Protect against cross-site request forgery
        public ActionResult Create(MarginViewModel margin)
        {
            if (ModelState.IsValid)
            {
                _margins.Add(margin);

                return RedirectToAction("Index"); //  Redirect to the Index action
            }

            // If ModelState is not valid, the form is redisplayed with validation errors.
            return View(margin);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var margin = _margins.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return View(margin);
        }

        // POST: My/Update/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, MarginViewModel margin)
        {
            if (id != margin.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //_context.Update(myModel);
                //_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(margin);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var margin = _margins.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return View(margin);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var margin = _margins.FirstOrDefault(x => x.ID == id);
            if (margin == null)
            {
                return NotFound();
            }

            return View(margin);
        }

        // POST: My/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, MarginViewModel margin)
        {
            if (id != margin.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //_context.Update(myModel);
                //_context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(margin);
        }
    }
}
