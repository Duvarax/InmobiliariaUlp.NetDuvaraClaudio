
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.LoginPath = "/Usuario/Login";
    options.LogoutPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/Restringido";
});

builder.Services.AddAuthorization(options =>
{;
	options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "Empleado"));
});


    

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "logout",
    pattern: "{controller=Usuario}/{action=Logout}");

app.MapControllerRoute(
    name: "Cambiar Avatar",
    pattern: "{controller=Usuario}/{action=CambiarAvatar}/{id?}"
);
app.MapControllerRoute(
    name: "Cambiar Contraseña",
    pattern: "{controller=Usuario}/{action=CambiarContraseña}/{id?}"
);
app.MapControllerRoute(
    name: "inmueble",
    pattern: "{controller=Propietario}/{action=listar}",
    defaults: new {Controller = "Propietario", action = "listar"}
);
app.MapControllerRoute(
    name:"listarxpropietario",
    pattern:"{controller=Inmueble}/{action=IndexByEstadoPorUsuario}/{id}",
    defaults: new {Controller = "Inmueble", action = "IndexByEstadoPorUsuario"});
app.MapControllerRoute(
name:"listarxinmueble",
pattern:"{controller=Contrato}/{action=IndexPorInmueble}/{id}",
defaults: new {Controller = "Contrato", action = "IndexPorInmueble"});
app.MapControllerRoute(
name:"listarcontratoxfecha",
pattern:"{controller=Contrato}/{action=IndexPorFecha}",
defaults: new {Controller = "Contrato", action = "IndexPorFecha"});
app.MapControllerRoute(
name:"listarpagoxcontrato",
pattern:"{controller=Pago}/{action=IndexPorContrato}/{id}",
defaults: new {Controller = "Pago", action = "IndexPorContrato"});



app.Run();
