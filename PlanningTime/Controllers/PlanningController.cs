using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningTime.Models;
using PlanningTime.Services;

namespace PlanningTime.Controllers
{
    public class PlanningController : Controller
    {
        private readonly PlanningService _planningService;
        private readonly PlanningDbContext _context;


        public PlanningController(PlanningDbContext context, PlanningService planningService)
        {
            _context = context;
            _planningService = planningService;
        }

        public IActionResult Index(int? userId, int year = 0, int month = 0)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            // ⚡ L'utilisateur réellement connecté
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (!sessionUserId.HasValue)
                return RedirectToAction("Login", "Account");

            // ⚡ userId d'affichage : soit paramètre, soit celui connecté
            int displayUserId = userId ?? sessionUserId.Value;


            var model = _planningService.GetMonthPlanning(year, month, displayUserId);

            // ⚡ Charger la liste des utilisateurs
            var users = _context.Users
        .Select(u => new User
        {
            Id = u.Id,
            LastName = u.LastName,
            FirstName = u.FirstName
        })
        .ToList();
            ViewBag.Users = users;

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = GetMonthName(month);
            return View(model);
        }

       
        [HttpGet]
        public IActionResult GetMonth(int year, int month, int? userId)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            // ⚡ L'utilisateur réellement connecté
            var sessionUserId = HttpContext.Session.GetInt32("UserId");
            if (!sessionUserId.HasValue)
                return RedirectToAction("Login", "Account");

            // ⚡ userId d'affichage : soit paramètre, soit celui connecté
            int displayUserId = userId ?? sessionUserId.Value;

            var model = _planningService.GetMonthPlanning(year, month, displayUserId);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = GetMonthName(month);
            return PartialView("_PlanningGrid", model);
        }

        public IActionResult UsersPlanning(int year = 0, int month = 0)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            var model = _planningService.GetMonthPlanningForAllUsers(year, month);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = GetMonthName(month);
            return View(model);
        }

        [HttpGet]
        public IActionResult GetUsersMonth(int year, int month)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            var model = _planningService.GetMonthPlanningForAllUsers(year, month);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = GetMonthName(month);
            return PartialView("_UsersPlanningGrid", model);
        }

        private string GetMonthName(int month)
        {
            return new DateTime(2000, month, 1).ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));
        }

    }





}
