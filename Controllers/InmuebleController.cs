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
        private RepositorioPropietario repositorioPropietario;
        public InmuebleController()
        {
            repo = new RepositorioInmueble();
            repositorioPropietario = new RepositorioPropietario();
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
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            try
            {
                // TODO: Add insert logic here
                int res = repo.agregarInmueble(inmueble);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    
                    return View();
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                // TODO: Add update logic here
                inmueble.Id = id;
                int res = repo.modificarInmueble(inmueble);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        public ActionResult Delete(int id)
        {
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repo.eliminarInmuebleById(id);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
    }
}