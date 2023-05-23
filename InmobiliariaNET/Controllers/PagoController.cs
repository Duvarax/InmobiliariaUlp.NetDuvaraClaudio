using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PracticaMVC.Models;
namespace PracticaMVC.Controllers
{
    public class PagoController : Controller
    {
        private RepositorioPagos rpo;
        private RepositorioContrato rpoContrato;
        private readonly IConfiguration config;
        public PagoController(IConfiguration config)
        {
            this.config = config;
            rpo = new RepositorioPagos();
            rpoContrato = new RepositorioContrato(config);
        }
        // GET: Pago
        [Authorize]
        public ActionResult Index()
        {
            List<Pago> listaPagos = rpo.GetPagos();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.EliminacionExitosa = TempData["EliminacionExitosa"];
            ViewBag.NoPermitido = TempData["NoPermitido"];
            ViewBag.Error = TempData["Error"];
            ViewBag.Contratos = rpoContrato.GetContratos();
            return View(listaPagos);
        }

        // GET: Pago/IndexPorContrato/5
        [Authorize]
        public ActionResult IndexPorContrato(int id)
        {
            List<Pago> listaPagos = rpo.GetPagosPorContrato(id);
            ViewBag.Contratos = rpoContrato.GetContratos();
            return View("Index", listaPagos);
        }

        [Authorize]
        public ActionResult Pagar(int id){
            ViewBag.Contratos = rpoContrato.GetContratos();
            Pago pago = rpo.obtenerPagoByIdDeContrato(id);
            return View("Create", pago);
        }


        // GET: Pago/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Pago pago = rpo.obtenerPagoById(id);
            return View(pago);
        }

        // GET: Pago/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Error = TempData["Error"];
            ViewBag.Contratos = rpoContrato.GetContratos();
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago pago)
        {
            ViewBag.Contratos = rpoContrato.GetContratos();
            try
            {
                if(pago.IdentificadorPago == null || pago.IdentificadorPago == 0){
                    pago.IdentificadorPago = 1;
                }else{
                    pago.IdentificadorPago++;
                }
                
                int res = rpo.agregarPago(pago);
                if(res > 0){
                    TempData["CreacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["Error"] = "Error en la creacion";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = e+"";
                 return RedirectToAction(nameof(Create));
            }
        }

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
           
            Pago pago = rpo.obtenerPagoById(id);
            ViewBag.Error = TempData["Error"];
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                // TODO: Add update logic here
                pago.Id = id;
                int res = rpo.modificarPago(pago);
                if(res > 0){
                    TempData["ModificacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["Error"] = "Error en la modificacion";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = "Parametros nulos";
                return RedirectToAction(nameof(Edit));
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            Pago pago = rpo.obtenerPagoById(id);
            ViewBag.Error = TempData["Error"];
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = rpo.eliminarPagoById(id);
                if(res > 0){
                    TempData["EliminacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    throw new ArgumentException("Un parametro es nulo");
                }
                
            }
            catch(Exception e) 
            {
                TempData["Error"] = 1;
                return View();
            }
        }
    }
}