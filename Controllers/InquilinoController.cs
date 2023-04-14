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
    public class InquilinoController : Controller
    {
        RepositorioInquilino ri;
        // GET: Inquilino

        public InquilinoController()
        {
            ri = new RepositorioInquilino();
        }
        [Authorize]
        public ActionResult Index()
        {   
            List<Inquilino> inquilinos =  ri.GetInquilinos();
            return View(inquilinos);
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Inquilino inquilino = ri.obtenerInquilinoById(id);
            return View(inquilino);
        }

        // GET: Inquilino/Create
        [Authorize]
        public ActionResult Create()
        {

            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                // TODO: Add insert logic here
                int res = ri.agregarInquilino(inquilino);
                if(res > 0){
                    return RedirectToAction(nameof(Index));
                }else{
                    return View();
                }
                
            }
            catch(Exception ex)
            {   
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Inquilino inquilino = ri.obtenerInquilinoById(id);
            return View(inquilino);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                // TODO: Add update logic here
                inquilino.Id = id;
                int res = ri.modificarInquilino(inquilino);
                if(res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }
                
            }
            catch(Exception ex)
            {   
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Inquilino inquilino = ri.obtenerInquilinoById(id);
            return View(inquilino);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                int res = ri.eliminarInquilinoById(id);
                if(res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View();
                }

                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }
    }
}