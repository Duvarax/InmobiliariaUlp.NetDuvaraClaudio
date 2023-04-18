using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {   
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            List<Inmueble> listaInmuebles = repo.GetInmuebles();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.EliminacionExitosa = TempData["EliminacionExitosa"];
            return View(listaInmuebles);
        }
        [Authorize]
        public ActionResult IndexByEstadoDisponible(){
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            List<Inmueble> listaInmueblesDisponibles = repo.GetInmueblesByDisponibilidad();
            return View("Index", listaInmueblesDisponibles);
        }
        [Authorize]
        public ActionResult IndexByEstadoNoDisponible(){
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            List<Inmueble> listaInmueblesNoDisponbiles = repo.GetInmueblesByNoDisponibilidad();
            return View("Index", listaInmueblesNoDisponbiles);
        }
        [Authorize]
        public ActionResult IndexByEstadoPorUsuario(int id){
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            List<Inmueble> listaInmueblesNoDisponbiles = repo.GetInmueblesPorPropietario(id);
            return View("Index", listaInmueblesNoDisponbiles);
        }

        // GET: Inmueble/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // GET: Inmueble/Create
        [Authorize]
        public ActionResult Create()
        {
            
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            ViewBag.Error = TempData["Error"];
            return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            try
            {
                // TODO: Add insert logic here
                int res = repo.agregarInmueble(inmueble);
                if(res > 0){
                    TempData["CreacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    throw new ArgumentException("Un parametro es nulo");
                }

            }
            catch(Exception e){
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Create));
            }
        }

        // GET: Inmueble/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewBag.Propietarios = repositorioPropietario.GetPropietarios();
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                // TODO: Add update logic here
                inmueble.Id = id;
                int res = repo.modificarInmueble(inmueble);
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
                return RedirectToAction("Index");
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Inmueble inmueble = repo.obtenerInmuebleById(id);
            return View(inmueble);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repo.eliminarInmuebleById(id);
                if(res > 0){
                    TempData["EliminacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = 1;
                return RedirectToAction("Index");
            }
        }

        public IList<Inmueble> listarInmuebles()
        {
            return repo.GetInmuebles();
        }
    }
}