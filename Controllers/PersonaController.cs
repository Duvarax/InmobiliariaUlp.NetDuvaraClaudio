using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
namespace PracticaMVC.Controllers
{

    public class PersonaController : Controller
    {
        private readonly RepositorioPersona? repositorioPersona;
        public PersonaController()
        {
            repositorioPersona = new RepositorioPersona();
        }
        // GET: Persona
        public ActionResult Index()
        {
            List<Persona> lista = repositorioPersona.GetPersonas();
            return View(lista);
        }

        // GET: Persona/Details/5
        public ActionResult Details(int id)
        {
            Persona persona = repositorioPersona.obtenerPersonaById(id);
            return View(persona);
        }

        // GET: Persona/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Persona/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Persona persona)
        {
            try
            {
                // TODO: Add insert logic here
                int res = repositorioPersona.agregarPersona(persona);
                Console.WriteLine(res);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(persona);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
        // GET: Persona/Edit/5
        public ActionResult Edit(int id)
        {
            Persona persona = repositorioPersona.obtenerPersonaById(id);
            return View(persona);
        }


        // POST: Persona/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Persona persona)
        {
            try
            {
                // TODO: Add update logic here
                persona.Id = id;
                repositorioPersona.modificarPersona(persona);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Persona/Delete/5
        public ActionResult Delete(int id)
        {
            Persona persona = repositorioPersona.obtenerPersonaById(id);
            return View(persona);
        }

        // POST: Persona/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                int res = repositorioPersona.eliminarPersonaById(id);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
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