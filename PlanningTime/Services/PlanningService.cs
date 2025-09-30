using Microsoft.EntityFrameworkCore;
using PlanningTime.Models;

namespace PlanningTime.Services
{
    public class PlanningService
    {
        private readonly HolidayService _holidayService;
        private readonly PlanningDbContext _context;


        public PlanningService(HolidayService holidayService, PlanningDbContext context)
        {
            _holidayService = holidayService;
            _context = context;
        }

        public List<PlanningDay> GetMonthPlanning(int year, int month, int userId)
        {
            var days = new List<PlanningDay>();
            var holidays = _holidayService.GetHolidays(year);

            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // Récupérer uniquement les événements de l’utilisateur connecté
            var events = _context.Events
                .Include(e => e.User)
                .Include(e => e.EventType)
                .Where(e => e.UserId == userId &&
                            e.Status == EventStatus.Approved &&
                            e.IsDeleted == false &&
                            e.StartDate <= lastDay &&
                            e.EndDate >= firstDay)
                .ToList();

            for (var date = firstDay; date <= lastDay; date = date.AddDays(1))
            {
                var day = new PlanningDay
                {
                    Date = date,
                    IsDisabled = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday
                                 || holidays.Contains(date.Date)
                };

                // Si l’utilisateur a un événement ce jour-là
                var evt = events.FirstOrDefault(e => e.StartDate <= date && e.EndDate >= date);
                if (evt != null)
                {
                    day.Type = MapEventTypeToDayType(evt.EventType.Name);
                    day.EventId = evt.Id;
                    day.EventTypeId = evt.EventTypeId;
                    day.UserEvents.Add(new UserEvent
                    {
                        UserId = evt.UserId,
                        UserName = evt.User.FirstName + " " + evt.User.LastName,
                        Type = day.Type.Value,
                        EventId = day.EventId,          // 👈 ici on met l’ID de l’événement
                        EventTypeId = evt.EventTypeId,

                    });
                }

                days.Add(day);
            }

            return days;
        }


        // ✅ Planning pour tous les utilisateurs d’un mois
        public List<UserPlanning> GetMonthPlanningForAllUsers(int year, int month)
        {
            var users = _context.Users.ToList();
            var holidays = _holidayService.GetHolidays(year);

            var firstDay = new DateTime(year, month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            // Récupérer tous les événements du mois
            var events = _context.Events
                .Include(e => e.User)
                .Include(e => e.EventType)
                .Where(e => e.StartDate <= lastDay && e.EndDate >= firstDay)
                .ToList();

            var result = new List<UserPlanning>();

            foreach (var user in users)
            {
                var userPlanning = new UserPlanning
                {
                    UserId = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    ImageUser = user.ImageUser
                };

                var weeks = new List<WeekPlanning>();

                // 🔹 Nombre total de jours du mois
                int totalDays = (lastDay - firstDay).Days + 1;

                // 🔹 Nombre total de semaines nécessaires
                int totalWeeks = (int)Math.Ceiling(totalDays / 7.0);

                for (int w = 0; w < totalWeeks; w++)
                {
                    var week = new WeekPlanning();

                    for (int d = 0; d < 7; d++)
                    {
                        var currentDate = firstDay.AddDays(w * 7 + d);
                        if (currentDate > lastDay) break;

                        var day = new PlanningDay
                        {
                            Date = currentDate,
                            IsHoliday = holidays.Contains(currentDate.Date),
                            IsWeekend = currentDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                        };

                        // Vérifie si cet utilisateur a un événement ce jour-là
                        var evt = events.FirstOrDefault(e =>
                            e.UserId == user.Id &&
                            e.StartDate <= currentDate &&
                            e.Status ==  EventStatus.Approved &&
                            e.IsDeleted == false &&
                            e.EndDate >= currentDate);

                        if (evt != null)
                        {
                            day.Type = MapEventTypeToDayType(evt.EventType.Name);
                            day.EventId = evt.Id;
                            day.EventTypeId = evt.EventTypeId;
                            day.UserEvents.Add(new UserEvent
                            {
                                UserId = evt.UserId,
                                UserName = $"{evt.User.FirstName} {evt.User.LastName}",
                                Type = day.Type.Value,
                                EventId = day.EventId,
                                EventTypeId = evt.EventTypeId,
                            });
                        }

                        // ✅ Couleur automatique
                        day.Color = GetColorForDay(day);

                        week.Days.Add(day);
                    }

                    weeks.Add(week);
                }

                userPlanning.Weeks = weeks;
                result.Add(userPlanning);
            }

            return result;
        }


        // ✅ Mappe un type d’évènement en DayType
        private DayType MapEventTypeToDayType(string eventTypeName)
        {
            return eventTypeName.ToLower() switch
            {
                "présentiel" => DayType.Presentiel,
                "télétravail" => DayType.Teletravail,
                "congé" => DayType.Conge,
                "absence" => DayType.Absence,
                "formation" => DayType.Formation,
                "férié" => DayType.Ferie,
                _ => DayType.Presentiel
            };
        }

        // ✅ Définit la couleur des boules
        private string GetColorForDay(PlanningDay day)
        {
            if (day.IsHoliday) return "#555555"; // gris foncé
            if (day.IsWeekend) return "#d3d3d3"; // gris clair

            if (day.Type != null)
            {
                return day.Type switch
                {
                    DayType.Presentiel => "#4CAF50", // vert
                    DayType.Teletravail => "#2196F3", // bleu
                    DayType.Conge => "#FF9800", // orange
                    DayType.Absence => "#9C27B0", // violet
                    DayType.Formation => "#00BCD4", // cyan
                    DayType.Ferie => "#555555", // férié (gris foncé)
                    _ => "#e0e0e0"
                };
            }

            return "#e0e0e0"; // par défaut
        }


    }


}
