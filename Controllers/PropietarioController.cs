using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
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
        public ActionResult Index()
        {
            List<Propietario> lista = rp.GetPropietarios();
            return View(lista);
        }

        // GET: Propietario/Details/5
        public ActionResult Details(int id)
        {
            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // GET: Propietario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                // TODO: Add insert logic here
                int res = rp.agregarPropietario(propietario);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Edit/5
        public ActionResult Edit(int id)
        {
            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                // TODO: Add update logic here
                propietario.Id = id;
                int res = rp.modificarPropietario(propietario);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Delete/5
        public ActionResult Delete(int id)
        {

            Propietario propietario = rp.obtenerPropietarioById(id);
            return View(propietario);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = rp.eliminarPropietarioById(id);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch
            {
                return View();
            }
        }
    }
}