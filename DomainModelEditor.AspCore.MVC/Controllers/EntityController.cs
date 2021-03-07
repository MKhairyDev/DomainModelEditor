using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.AspCore.MVC.Controllers
{
    public class EntityController : Controller
    {
        // GET: EntityController
        public ActionResult Index()
        {
            return View();
        }

        // GET: EntityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EntityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntityController/Create
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

        // GET: EntityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EntityController/Edit/5
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

        // GET: EntityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EntityController/Delete/5
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
