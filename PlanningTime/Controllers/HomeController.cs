using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlanningTime.Models;
using PlanningTime.Services;
using System.Diagnostics;

namespace PlanningTime.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PlanningDbContext _context;
        private readonly PlanningService _planningService;

        public HomeController(ILogger<HomeController> logger, PlanningDbContext context, PlanningService planningService)
        {
            _logger = logger;
            _context = context;
            _planningService = planningService;
        }

        public IActionResult Index(int year = 0, int month = 0)
        {
            if (year <= 0) year = DateTime.Now.Year;
            if (month < 1 || month > 12) month = DateTime.Now.Month;

            var events = GetEvents(year, month);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.MonthName = new DateTime(year, month, 1).ToString("MMMM", new System.Globalization.CultureInfo("fr-FR"));

            return View(events);
        }

        public IActionResult GetDashboard(int year, int month)
        {
            var events = GetEvents(year, month);
            return PartialView("_DashboardContent", events);
        }

        private List<Event> GetEvents(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var events = _context.Events
                .Include(e => e.EventType)
                .Where(e => e.StartDate <= lastDay && e.EndDate >= firstDay && !e.IsDeleted)
                .ToList();

            var statsByType = events
                .GroupBy(e => e.EventType.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            var statsByStatus = events
                .GroupBy(e => e.Status)
                .ToDictionary(g => g.Key.ToString(), g => g.Count());

            ViewBag.StatsByType = statsByType;
            ViewBag.StatsByStatus = statsByStatus;
            ViewBag.TotalEvents = events.Count;

            return events;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
