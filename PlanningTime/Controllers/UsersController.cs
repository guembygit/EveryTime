using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningTime.Data;
using PlanningTime.Models;
using System.Security.Cryptography;
using System.Text;

[Authorize(Roles = "Admin")] // Seul Admin accède
public class UsersController : Controller
{
    private readonly PlanningDbContext _context;

    public UsersController(PlanningDbContext context)
    {
        _context = context;
    }

    // Liste des utilisateurs
    public IActionResult Index()
    {
        ViewBag.Profiles = _context.Profiles.ToList();
        var users = _context.Users.Include(u => u.Profile).ToList();
        return View(users);
    }

    // GET: Partial view pour édition modal
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        ViewBag.Profiles = _context.Profiles.ToList();
        return PartialView("_EditUserModal", user); // PartialView pour le modal
    }

    // POST: Création utilisateur
    [HttpPost]
    public IActionResult Create(User model)
    {
        model.PasswordHash = HashPassword(Request.Form["password"]);
        _context.Users.Add(model);
        try
        {
            _context.SaveChanges();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Erreur DB: " + ex.Message });
        }
    }


    public IActionResult ListPartial()
    {
        var users = _context.Users.Include(u => u.Profile).ToList();
        return PartialView("_UsersTablePartial", users);
    }


    // POST: Edition utilisateur via AJAX
    [HttpPost]
    public IActionResult Edit(User model)
    {
        if (!ModelState.IsValid) return Json(new { success = false, message = "Données invalides" });

        var user = _context.Users.Find(model.Id);
        if (user == null) return Json(new { success = false, message = "Utilisateur introuvable" });

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.ProfileId = model.ProfileId;
        user.HireDate = model.HireDate;
        user.IsActive = model.IsActive;

        _context.SaveChanges();
        return Json(new { success = true });
    }

    // Hashage mot de passe sécurisé
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    
    // Activer/Désactiver via Ajax
    [HttpPost]
    public IActionResult ToggleStatus(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.IsActive = !user.IsActive;
        _context.SaveChanges();

        return Json(new { success = true, isActive = user.IsActive });
    }

    public IActionResult GetUserImage(int id)
    {
        var user = _context.Users.Find(id);
        if (user?.ImageUser == null || user.ImageUser.Length == 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/default-user.png");
            var defaultBytes = System.IO.File.ReadAllBytes(path);
            return File(defaultBytes, "image/png");
        }

        return File(user.ImageUser, "image/png");
    }


}
