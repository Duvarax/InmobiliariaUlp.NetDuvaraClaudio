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
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
            ViewBag.EliminacionExitosa = TempData["EliminacionExitosa"];
            ViewBag.Multa = TempData["Multa"];
            ViewBag.Renovar = TempData["Renovar"];
            ViewBag.Error = TempData["Error"];
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


        // POST: Contrato/IndexPorFecha
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult IndexPorFechas(IFormCollection collection){

            string fechaDesde = collection["desde"];
            string fechaHasta = collection["hasta"];

            if(fechaDesde != "" && fechaHasta != ""){
                ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
                List<Contrato> listaContratos = repositorioContrato.GetContratosPorFechas(DateTime.Parse(fechaDesde), DateTime.Parse(fechaHasta));
                return View("Index", listaContratos);
            }else{
                TempData["Error"] = "Fechas vacias";
                return RedirectToAction(nameof(Index));
            }

            
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
                contrato.Estado = true;
                // TODO: Add insert logic here
                ViewBag.Inquilinos = repositorioInquilino.GetInquilinos();
                ViewBag.Inmuebles = repositorioInmueble.GetInmuebles();
                int res = repositorioContrato.agregarContrato(contrato);
                if(res > 0){
                    TempData["CreacionExitosa"] = contrato.Id;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["Error"] = "Campos no completados correctamente";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = "Campos vacios";
                return RedirectToAction(nameof(Index));
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
                
                if(contrato.InmuebleId != null || contrato.InmuebleId != null){
                    int res = repositorioContrato.modificarContrato(contrato);
                    if(res > 0){
                        TempData["ModificacionExitosa"] = 1;
                        return RedirectToAction(nameof(Index));
                    }
                }else{
                   TempData["Error"] = "Campos no completados correctamente";
                    return RedirectToAction(nameof(Index)); 
                }
                return RedirectToAction(nameof(Index)); 
            }
            catch(Exception e)
            {
                TempData["Error"] = "Campos no completados correctamente";
                return RedirectToAction(nameof(Index));
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
                    TempData["Error"] = "Error de eliminacion";
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch(Exception e)
            {
                TempData["Error"] = "Error de eliminacion";
                return View();
            }
        }
        [Authorize]
        public IActionResult Cancelar(int id){
            
            try
            {
                Contrato contrato = repositorioContrato.obtenerContratoById(id);
                ViewBag.Cancelar = "Cancelar";
                return View("Delete", contrato);
            }
            catch (Exception)
            {
                
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public IActionResult Cancelar(int id, IFormCollection collection){

            try
            {
            Contrato contrato = repositorioContrato.obtenerContratoById(id);
            Double multa;
            DateTime hoy = DateTime.Today;
            DateTime fechaFinalizacion = (DateTime)contrato.fechaFinalizacion;
            TimeSpan tiempoDiferencia = fechaFinalizacion.Subtract(hoy);
            int mesesDiferencia = (int) Math.Round(tiempoDiferencia.TotalDays / 30.0);
        
            if (mesesDiferencia == 1)
            {
                multa = (double)(contrato.Precio / 30.0);
            }else
            {
                multa = (double)((contrato.Precio / 30.0) * 2);

            }
            int res = repositorioContrato.cancelarContrato(contrato);
            if(res > 0){
                TempData["Multa"] = Math.Round(multa, 2) + "";
                return RedirectToAction(nameof(Index));
            }
            return View();
            }
            catch (Exception)
            {
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        public IActionResult Renovar(int id)
        {
             try
            {
                Contrato contrato = repositorioContrato.obtenerContratoById(id);
                ViewBag.Renovar = "Renovar";
                return View(contrato);
            }
            catch (Exception)
            {
                
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Renovar(int id, Contrato contrato)
        {
            try
            {
                contrato.Id = id;
                int res = repositorioContrato.renovarContrato(contrato);
                if(res > 0){
                    TempData["Renovar"] = 1;
                    return RedirectToAction(nameof(Index));
                }
                return View(Index);
            }
            catch (Exception)
            {
                
                TempData["Error"] = 1;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}