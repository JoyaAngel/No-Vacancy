using Microsoft.EntityFrameworkCore;
using NoVacancy.Data;
using NoVacancy.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NoVacancyDbContex>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NoVacancy")));

builder.Services.AddControllersWithViews();

//Añadir identity
builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<NoVacancyDbContex>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//Identity.
app.UseAuthorization();
app.UseAuthentication();

app.Run();
