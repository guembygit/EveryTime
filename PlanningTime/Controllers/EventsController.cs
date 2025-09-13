using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlanningTime.Models;

// USING A AJOUTER
using Microsoft.EntityFrameworkCore;

namespace PlanningTime.Controllers
{
    public class EventsController : Controller
    {
        private readonly PlanningDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EventsController(PlanningDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.EventTypes = _context.EventTypes.ToList();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                // Renvoie uniquement le formulaire partial pour AJAX
                return PartialView("_CreateEventForm", new CreateEventViewModel());
            }

            return View(new CreateEventViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Upload justificatif
                string? justificatifPath = null;
                if (model.Justificatif != null)
                {
                    string uploads = Path.Combine(_env.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploads);
                    string filePath = Path.Combine(uploads, Guid.NewGuid() + Path.GetExtension(model.Justificatif.FileName));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Justificatif.CopyToAsync(stream);
                    }
                    justificatifPath = "/uploads/" + Path.GetFileName(filePath);
                }

                // Récupérer l’utilisateur connecté (simplifié)
                var user = _context.Users.FirstOrDefault(u => u.Email == User.Identity.Name);

                var newEvent = new Event
                {
                    UserId = user.Id,
                    EventTypeId = model.EventTypeId,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Motif = model.Motif,
                    Justificatif = justificatifPath,
                    Status = EventStatus.Pending,
                    CreatedAt = DateTime.Now
                };

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyEvents");
            }
            return View(model);
        }

        public IActionResult MyEvents()
        {
            // Récupération du login de l'utilisateur connecté
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized(); // pas connecté
            }

            // Charger l'utilisateur
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return Unauthorized(); // pas trouvé
            }

            // Charger ses événements
            var events = _context.Events
                .Include(e => e.EventType)
                .Where(e => e.UserId == user.Id)
                .ToList();

            return View(events);
        }

        // Annuler / supprimer
        public IActionResult CancelOrDelete(DateTime date)
        {
            var evt = _context.Events.FirstOrDefault(e => e.StartDate.Date == date.Date);
            return PartialView("_CancelOrDeleteForm", evt);
        }

        [HttpPost]
        public IActionResult CancelOrDeleteConfirmed(int id)
        {
            var evt = _context.Events.Find(id);
            if (evt != null)
            {
                _context.Events.Remove(evt);
                _context.SaveChanges();
            }
            return Ok();
        }

        // ⚡ Nouveau : Modifier ou annuler (Présentiel, Télétravail, Absence)
        public IActionResult Manage(DateTime date, string type)
        {
            var evt = _context.Events.FirstOrDefault(e => e.StartDate.Date == date.Date);
            return PartialView("_ManageEventForm", evt);
        }

        [HttpPost]
        public IActionResult Manage(Event model, string actionType)
        {
            var evt = _context.Events.Find(model.Id);
            if (evt == null) return NotFound();

            if (actionType == "update")
            {
                evt.EventType = model.EventType;
                _context.SaveChanges();
            }

            else if (actionType == "delete")
            {
                _context.Events.Remove(evt);
                _context.SaveChanges();
            }

            return Ok();
        }

    }

}
