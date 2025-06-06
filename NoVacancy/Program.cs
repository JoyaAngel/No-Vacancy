using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NoVacancy.Data;
using NoVacancy.Models;
using NoVacancy.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NoVacancyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoVacancy")));

// builder.Services.AddControllersWithViews();

//Agregar identity
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<NoVacancyDbContext>()
    .AddDefaultTokenProviders();



//Filtro global de autorización.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

builder.Services.AddSession();

//Redirigir al login si no hay sesión activa.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Usuario/Login";
    options.AccessDeniedPath = "/Usuario/AccessDenied";
});

// Configuración de EmailSettings y registro del servicio de email
builder.Services.Configure<NoVacancy.Services.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

//Usar seeder.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedUserRoles.Initialize(services);
    SeederDB.SeedAll(services); // Llama al seeder general
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();
app.UseSession();

//Identity.
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



app.Run();
