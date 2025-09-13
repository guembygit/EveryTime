using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanningTime.Models;

// USING A AJOUTER
using Microsoft.EntityFrameworkCore;


namespace PlanningTime.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminEventsController : Controller
    {
        private readonly PlanningDbContext _context;

        public AdminEventsController(PlanningDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pendingEvents = _context.Events
                .Include(e => e.User)
                .Include(e => e.EventType)
                .Where(e => e.Status == EventStatus.Pending)
                .ToList();

            return View(pendingEvents);
        }

        [HttpPost]
        public IActionResult Validate(int id, bool approve)
        {
            var ev = _context.Events.Include(e => e.User).FirstOrDefault(e => e.Id == id);
            if (ev == null) return NotFound();

            ev.Status = approve ? EventStatus.Approved : EventStatus.Rejected;
            ev.ValidatedById = _context.Users.First(u => u.Email == User.Identity.Name).Id;
            ev.ValidatedAt = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }

}
