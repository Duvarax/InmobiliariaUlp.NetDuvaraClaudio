using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
namespace PracticaMVC.Controllers
{
    public class ContratoController : Controller
    {
        private RepositorioContrato repositorioContrato;
        private RepositorioInmueble repositorioInmueble;
        private RepositorioInquilino repositorioInquilino;
        public ContratoController()
        {
            repositorioContrato = new RepositorioContrato();
            repositorioInmueble = new RepositorioInmueble();
            repositorioInquilino = new RepositorioInquilino();
        }
        // GET: Contrato
        public ActionResult Index()
        {
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            
            List<Contrato> listaContratos = repositorioContrato.GetContratos();
            return View(listaContratos);
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int id)
        {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            return View(contrato);
        }

        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioContrato.agregarContrato(contrato);
                if(res > 0){
                    TempData["CreacionExitosa"] = contrato.Id;
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

        // GET: Contrato/Edit/5
        public ActionResult Edit(int id)
        {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            return View(contrato);
        }

        // POST: Contrato/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            try
            {
                // TODO: Add update logic here
                contrato.Id = id;
                int res = repositorioContrato.modificarContrato(contrato);
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

        // GET: Contrato/Delete/5
        public ActionResult Delete(int id)
        {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            return View(contrato);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repositorioContrato.eliminarContratoById(id);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return RedirectToAction(nameof(Index));
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