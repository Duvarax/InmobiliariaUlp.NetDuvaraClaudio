
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options => {
    options.LoginPath = "/Usuario/Login";
    options.LoginPath = "/Usuario/Logout";
    options.AccessDeniedPath = "/Home/Restringido";
});

builder.Services.AddAuthorization(options => {
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

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "logout",
    pattern: "{controller=Usuario}/{action=Logout}");
app.MapControllerRoute(
    name: "inmueble",
    pattern: "{controller=Propietario}/{action=listar}",
    defaults: new {Controller = "Propietario", action = "listar"}
);

app.Run();
