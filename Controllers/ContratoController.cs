using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.CreacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            List<Contrato> listaContratos = repositorioContrato.GetContratos();
            return View(listaContratos);
        }
        [Authorize]
        public ActionResult IndexVigentes()
        {
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            List<Contrato> listaContratos = repositorioContrato.GetContratosVigentes();
            return View("Index", listaContratos);
        }
        // GET: Contrato/IndexPorInmueble/5
        [Authorize]
         public ActionResult IndexPorInmueble(int id){
             ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
             List<Contrato> listaContratos = repositorioContrato.GetContratosPorInmueble(id);
             return View("Index", listaContratos);
         }

        // GET: Contrato/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            return View(contrato);
        }

        // GET: Contrato/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
                TempData["Error"] = 1;
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Contrato/Edit/5
        [Authorize]
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
        [Authorize]
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
                    TempData["ModificacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    
                    return View();
                }
            }
            catch(Exception e)
            {
                TempData["Error"] = 1;
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Contrato/Delete/5
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            return View(contrato);
        }

        // POST: Contrato/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repositorioContrato.eliminarContratoById(id);
                if(res > 0){
                    TempData["EliminacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = 1;
                Console.WriteLine(e);
                return View();
            }
        }
    }
}