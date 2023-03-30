using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
namespace PracticaMVC.Controllers
{
    public class InmuebleController : Controller
    {
        private RepositorioInmueble repo;
        public InmuebleController()
        {
            repo = new RepositorioInmueble();
        }
        // GET: Inmueble
        public ActionResult Index()
        {   
            List<Inmueble> listaInmuebles = repo.GetInmuebles();
            return View(listaInmuebles);
        }

        // GET: Inmueble/Details/5
        public ActionResult Details(int id)
        {
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}