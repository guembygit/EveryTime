using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PlanningTime.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using PlanningTime.Data;
    using PlanningTime.Models;
    using System.Text;
    using System.Security.Cryptography;
    using Microsoft.EntityFrameworkCore;

    public class AccountController : Controller
    {
        private readonly PlanningDbContext _context;

        public AccountController(PlanningDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashed = HashPassword(password).Trim();

            var user = _context.Users
                .Include(u => u.Profile)
                .FirstOrDefault(u => u.Email.ToLower() == email.ToLower()
                                  && u.PasswordHash.Trim() == hashed.Trim());

            if (user == null)
            {
                ViewBag.Error = "Email ou mot de passe incorrect.";
                return View();
            }

            // Stocker l'ID utilisateur dans la session
            HttpContext.Session.SetInt32("UserId", user.Id);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim("FullName", user.FirstName + " " + user.LastName),
        new Claim(ClaimTypes.Role, user.Profile != null ? user.Profile.Name : "Utilisateur")
    };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", principal);

            return RedirectToAction("Index", "Planning");
        }




        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Login");
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes); // ⚡ Base64, comme en base
        }


    }

}
