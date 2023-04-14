using Microsoft.AspNetCore.Mvc;
using PracticaMVC.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public ActionResult Index()
        {
            List<Usuario> listaUsuarios = rpo.GetUsuarios();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.NoPermitido = TempData["NoPermitido"];
            return View(listaUsuarios);
        }

        // GET: Usuario/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // GET: Usuario/Create
        [Authorize(Policy="Administrador")]
        public ActionResult Create()
        {
            ViewBag.roles = Usuario.getRoles();
            return View();
        }
        
        // POST: Usuario/Create
        [Authorize(Policy="Administrador")]
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
        [Authorize]
        public ActionResult Edit(int id)
        {
            int x = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
            if(id !=  x){
                TempData["NoPermitido"] = 1;
                return RedirectToAction("Index");
            }
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
                return RedirectToAction(nameof(Delete));
            }
        }
        
        //GET: Usuario/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        //POST: Usuario/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView login)
        {

            if(!ModelState.IsValid){
                return View();
            }
            string contraseñaHasheada = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: login.Contraseña,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256/8
            ));

            

            Usuario usuario = rpo.obtenerUsuarioByEmail(login.Email);
            
            if(usuario == null || usuario.Contraseña != contraseñaHasheada)
            {
                ModelState.AddModelError("invalid", "Contraseña o Email Incorrectos");
                return View();

            }
            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id+""),
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                new Claim(ClaimTypes.Role, usuario.RolNombre),
            };

            var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index", "Home");
				}
                
				
        

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");

        
    }

        //GET: Usuario/CambiarAvatar
        [Authorize]
        public IActionResult CambiarAvatar(int id)
        {   
            int x = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
            if(id !=  x){
                TempData["NoPermitido"] = 1;
                return RedirectToAction("Index");
            }
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        //POST: Usuario/CambiarAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CambiarAvatar(int id, Usuario user)
        {

            try{
                Usuario usuario = rpo.obtenerUsuarioById(id);
                usuario.Id = id;
                string wwwPath = enviroment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                string fileName = "profile_avatar_"+usuario.Id + Path.GetExtension(user.AvatarFile.FileName);
                usuario.Avatar = Path.Combine("/Uploads", fileName);
                string pathCompleto = Path.Combine(path, fileName);
                using(FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    user.AvatarFile.CopyTo(stream);
                }
                int res = rpo.modificarUsuario(usuario);
                if(res > 0)
                {
                    TempData["ModificacionExitosa"] = 1;
                    return RedirectToAction("Index");
                }
                return View();
            }catch(Exception ex){
                Console.WriteLine(ex);
                return View();
            }
        }

        
        //GET: Usuario/CambiarContraseña
        [Authorize]
        public IActionResult CambiarContraseña(int id){
            int x = Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value));
            if(id !=  x){
                TempData["NoPermitido"] = 1;
                return RedirectToAction("Index");
            }
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        //POST: Usuario/CambiarContraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CambiarContraseña(int id, IFormCollection collection)
        {
            if(!ModelState.IsValid){
                return View();
            }
            try{
                Usuario usuario = rpo.obtenerUsuarioById(id);
                string contraseña_vieja = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: collection["contraseña_vieja"],
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 /8));
            if(!(usuario.Contraseña == contraseña_vieja)){
                return View();
            }else{

                string contraseña_nueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: collection["Contraseña"],
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 /8));

                usuario.Contraseña = contraseña_nueva;
                int res = rpo.modificarUsuario(usuario);
                if(res > 0)
                {
                    TempData["ModificacionExitosa"] = 1;
                    return RedirectToAction(nameof(Index));
                }
            }
                return View();
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                TempData["Error"] = 1;
                return View();
            }
        }

        
        

}
}