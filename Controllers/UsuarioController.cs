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
        [Authorize(Policy="Administrador")]
        public ActionResult Index()
        {
            List<Usuario> listaUsuarios = rpo.GetUsuarios();
            ViewBag.CreacionExitosa = TempData["CreacionExitosa"];
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            ViewBag.EliminacionExitosa = TempData["EliminacionExitosa"];
            ViewBag.NoPermitido = TempData["NoPermitido"];
            ViewBag.NoPermiso = TempData["NoPermiso"];
            return View(listaUsuarios);
        }

        // GET: Usuario/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        [Authorize]
        public ActionResult Perfil()
        {
            Usuario usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
            return View(nameof(Details),usuario);
        }
        [Authorize]
        public ActionResult EditarPerfil()
        {
            Usuario usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
            return View(nameof(Edit),usuario);
        }


        // GET: Usuario/Create
        [Authorize(Policy="Administrador")]
        public ActionResult Create()
        {
            if(!User.IsInRole("Administrador")){
                TempData["NoPermiso"] = 1;
                return RedirectToAction("Index");
            }
            ViewBag.roles = Usuario.getRoles();
            return View();
        }
        
        // POST: Usuario/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
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
        [Authorize(Policy="Administrador")]
        public ActionResult Edit(int id)
        {
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
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
                    return RedirectToAction(nameof(Perfil));
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
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id)
        {
            if(!User.IsInRole("Administrador")){
                TempData["NoPermiso"] = 1;
                return RedirectToAction("Index");
            }
            Usuario usuario = rpo.obtenerUsuarioById(id);
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {   
                if(!User.IsInRole("Administrador")){
                TempData["NoPermiso"] = 1;
                return RedirectToAction("Index");
                }
                Usuario usuario = rpo.obtenerUsuarioById(id);
                int res = rpo.eliminarUsuarioById(id);
                if(res > 0)
                {
                    var ruta = Path.Combine(enviroment.WebRootPath, "Uploads", "profile_avatar_"+id + Path.GetExtension(usuario.Avatar));
                    if (System.IO.File.Exists(ruta))
                        System.IO.File.Delete(ruta);
                    TempData["EliminacionExitosa"] = 1;
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
            ViewBag.LogError = TempData["LogError"];
            return View();
        }
        //POST: Usuario/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView login)
        {

            if(!ModelState.IsValid){
                TempData["LogError"] = 1;
                return RedirectToAction("Login");
                
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
                TempData["LogError"] = 1;
                return RedirectToAction("Login");

            }
            var claims = new List<Claim>
            {
                new Claim("Id", usuario.Id+""),
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("Email", usuario.Email),
                new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                new Claim(ClaimTypes.Role, usuario.RolNombre),
                new Claim("Rol", usuario.RolNombre),
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
            Usuario usuario = null;
            if(id == 0){
                usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
            }else if(User.IsInRole("Administrador")){
                usuario = rpo.obtenerUsuarioById(id);
            }
            ViewBag.ModificacionExitosa = TempData["ModificacionExitosa"];
            return View(usuario);
        }

        //POST: Usuario/CambiarAvatar
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CambiarAvatar(int id,Usuario user)
        {

            try{
                Usuario usuario = null;
                if(id == 0){
                    usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
                }else if(User.IsInRole("Administrador")){
                    usuario = rpo.obtenerUsuarioById(id);
                }
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
                     if(User.IsInRole("Administrador")){
                        TempData["ModificacionExitosa"] = 1;
                        return RedirectToAction(nameof(Index));
                    }else{
                        TempData["ModificacionExitosa"] = 1;
                        return RedirectToAction(nameof(Perfil));
                    }
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
            Usuario usuario = null;
                if(id == 0){
                    usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
                }else if(User.IsInRole("Administrador")){
                    usuario = rpo.obtenerUsuarioById(id);
                }
            ViewBag.Error = TempData["Error"];
            return View(usuario);
        }

        //POST: Usuario/CambiarContraseña
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult CambiarContraseña(int id,IFormCollection collection)
        {
            if(!ModelState.IsValid){
                return View();
            }
            try{
                Usuario usuario = null;
                if(id == 0){
                    usuario = rpo.obtenerUsuarioById(Int32.Parse((User.Claims.FirstOrDefault(c => c.Type == "Id").Value)));
                }else if(User.IsInRole("Administrador")){
                    usuario = rpo.obtenerUsuarioById(id);
                }
                string contraseña_vieja = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: collection["contraseña_vieja"],
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 /8));
            if(!(usuario.Contraseña == contraseña_vieja)){
                TempData["Error"] = "Contraseña antigua incorrecta";
                return RedirectToAction(nameof(CambiarContraseña));
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
                    if(User.IsInRole("Administrador")){
                        TempData["ModificacionExitosa"] = 1;
                        return RedirectToAction(nameof(Index));
                    }else{
                        TempData["ModificacionExitosa"] = 1;
                        return RedirectToAction(nameof(Perfil));
                    }
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