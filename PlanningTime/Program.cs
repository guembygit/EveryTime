using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlanningTime.Data;
using PlanningTime.Models;
using PlanningTime.Services;

var builder = WebApplication.CreateBuilder(args);

// ⚡ Ajoute ceci pour IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
builder.Services.AddSession(); // si tu utilises déjà la session
// ... autres services

// Ajouter les services, les injections de dépendences
builder.Services.AddScoped<HolidayService>();
builder.Services.AddScoped<PlanningService>();

// AJOUT DU CONTEXT EF CORE DANS L'INJECTION DE DEPENDANCES
builder.Services.AddDbContext<PlanningDbContext>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ajout du support des sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// On utilise cookies auth
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Denied";
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Appeler le seed au démarrage
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PlanningDbContext>();
    DbInitializer.Initialize(context);
}

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

// AUTHENTIFICATION IL VIENT TOUJOURS AVANT AUTORIZATION
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // ⚡ nécessaire pour les sessions



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
