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
        var users = _context.Users.Include(u => u.Profile).ToList();
        return View(users);
    }

    // Formulaire création
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Profiles = _context.Profiles.ToList(); // Rôles : Admin / Employé
        return View();
    }

    // Soumission création
    [HttpPost]
    public IActionResult Create(User model, string password)
    {
        if (ModelState.IsValid)
        {
            model.PasswordHash = HashPassword(password);
            model.IsActive = true; // par défaut actif
            model.HireDate = model.HireDate == default ? DateTime.Now : model.HireDate;

            _context.Users.Add(model);

            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Impossible de créer l’utilisateur : " + ex.Message);
            }
        }

        // Recharger la liste des profils si erreur
        ViewBag.Profiles = _context.Profiles.ToList();
        return View(model);
    }


    // Hashage mot de passe sécurisé
    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
