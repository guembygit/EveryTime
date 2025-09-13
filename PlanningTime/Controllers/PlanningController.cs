using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlanningTime.Models;
using PlanningTime.Services;

namespace PlanningTime.Controllers
{
    public class PlanningController : Controller
    {
        private readonly PlanningService _planningService;

        public PlanningController(PlanningService planningService)
        {
            _planningService = planningService;
        }

        public IActionResult Index(int? userId, int year = 0, int month = 0)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            // ⚡ Récupérer l'utilisateur connecté depuis la session
            userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");


            var model = _planningService.GetMonthPlanning(year, month, userId.Value);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = GetMonthName(month);
            return View(model);
        }

       
        [HttpGet]
        public IActionResult GetMonth(int year, int month)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var model = _planningService.GetMonthPlanning(year, month, userId.Value);

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
