
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PracticaMVC.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>//el sitio web valida con cookie
	{
		options.LoginPath = "/Usuario/Login";
		options.LogoutPath = "/Usuario/Logout";
		options.AccessDeniedPath = "/Home/Restringido";
		//options.ExpireTimeSpan = TimeSpan.FromMinutes(5);//Tiempo de expiración
	})
	.AddJwtBearer(options =>//la api web valida con token
	{
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = configuration["TokenAuthentication:Issuer"],
			ValidAudience = configuration["TokenAuthentication:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
				configuration["TokenAuthentication:SecretKey"])),
		};
		// opción extra para usar el token en el hub y otras peticiones sin encabezado (enlaces, src de img, etc.)
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				// Leer el token desde el query string
				var accessToken = context.Request.Query["access_token"];
				// Si el request es para el Hub u otra ruta seleccionada...
				var path = context.HttpContext.Request.Path;
				if (!string.IsNullOrEmpty(accessToken) &&
					(
					path.StartsWithSegments("/api/Propietario/reset") ||
					path.StartsWithSegments("/api/Propietario/token")))
				{//reemplazar las urls por las necesarias ruta ⬆
					context.Token = accessToken;
				}
				return Task.CompletedTask;
			}
		};
	});

    builder.Services.AddDbContext<DataContext>(
	options => options.UseSqlServer(
		configuration["ConnectionStrings:SQL"]
	)
);
    builder.Services.AddDbContext<DataContext>(
        options => options.UseMySql(
            configuration["ConnectionStrings:SQL"],
            ServerVersion.AutoDetect(configuration["ConnectionStrings:SQL"])
        )
    );


    

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
