using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
using Microsoft.AspNetCore.Authorization;
namespace PracticaMVC.Controllers
{
    public class PropietarioController : Controller
    {

        RepositorioPropietario rp;

        public PropietarioController()
        {
            rp = new RepositorioPropietario();
        }
        // GET: Propietario
        [Authorize]
        public ActionResult Index()
        {
            List<Propietario> lista = rp.GetPropietarios();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.EliminacionExitosa = TempData["EliminacionExitosa"];
            ViewBag.Error = TempData["Error"];
            return View(lista);
        }
        

        // GET: Propietario/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // GET: Propietario/Create
        [Authorize]
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                // TODO: Add insert logic here
                int res = rp.agregarPropietario(propietario);
                if(res > 0){
                    TempData["CreacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Propietario/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                // TODO: Add update logic here
                propietario.Id = id;
                int res = rp.modificarPropietario(propietario);
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
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {

            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = rp.eliminarPropietarioById(id);
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
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize]
        public List<Propietario> listar(){
            return rp.GetPropietarios();
        }
    }
}