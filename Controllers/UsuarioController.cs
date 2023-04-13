using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace PracticaMVC.Controllers
{
    public class UsuarioController : Controller
    {
        private RepositorioUsuario rpo;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment enviroment;
        public UsuarioController(IConfiguration configuration, IWebHostEnvironment enviroment)
        {
            this.configuration = configuration;
            this.enviroment = enviroment;
            rpo = new RepositorioUsuario();
        }
        // GET: Usuario
        public ActionResult Index()
        {
            List<Usuario> listaUsuarios = rpo.GetUsuarios();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            return View(listaUsuarios);
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.roles = Usuario.getRoles();
            return View();
        }
        
        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuario)
        {
            if(!ModelState.IsValid){
                return View();
            }
            try
            {
                string contraseñaHasheada = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Contraseña,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 /8));
                usuario.Contraseña = contraseñaHasheada;
                //usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int)enRoles.Empleado;
                int res = rpo.agregarUsuario(usuario);
                if(usuario.AvatarFile != null && res > 0)
                {
                    string wwwPath = enviroment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if(!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "profile_avatar_" + usuario.Id + Path.GetExtension(usuario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuario.Avatar = Path.Combine("/Uploads", fileName);
                    using(FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuario.AvatarFile.CopyTo(stream);
                    }
                    rpo.modificarUsuario(usuario);
                }
                TempData["CreacionExitosa"] = 1;
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return View();
            }
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                // TODO: Add update logic here
                if(!ModelState.IsValid){
                    return View();
                }
                usuario.Id = id;
                int res = rpo.modificarUsuario(usuario);
                if(res > 0){
                    TempData["ModificacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                TempData["Error"] = 1;
                return View();
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {   
                Usuario usuario = rpo.obtenerUsuarioById(id);
                int res = rpo.eliminarUsuarioById(id);
                if(res > 0)
                {
                    var ruta = Path.Combine(enviroment.WebRootPath, "Uploads", "profile_avatar_"+id + Path.GetExtension(usuario.Avatar));
                    if (System.IO.File.Exists(ruta))
                        System.IO.File.Delete(ruta);
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                TempData["Error"] = 1;
                return View();
            }
        }
        [Authorize]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return View();
        }

        
    }
}