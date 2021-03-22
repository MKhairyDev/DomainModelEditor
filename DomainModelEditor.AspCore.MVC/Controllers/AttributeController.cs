using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.AspCore.MVC.Controllers
{
    public class AttributeController : Controller
    {
        // GET: AttributeController
        public IActionResult Index(int entityId)
        {
            return View();
        }

        // GET: AttributeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AttributeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AttributeController/Create
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

        // GET: AttributeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AttributeController/Edit/5
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

        // GET: AttributeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AttributeController/Delete/5
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
