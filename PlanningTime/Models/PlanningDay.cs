namespace PlanningTime.Models
{
    public enum DayType
    {
        Presentiel,
        Teletravail,
        Conge,
        Absence,
        Formation,
        Ferie,
        Weekend
    }

    public class PlanningDay
    {
        public DateTime Date { get; set; }
        public DayType? Type { get; set; } // null si non renseigné
        public bool IsDisabled { get; set; } // Weekends ou fériés

        public bool IsHoliday { get; set; } = false;
        public bool IsWeekend { get; set; } = false;

        public List<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

        // ✅ Ajout des propriétés
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Couleur de la boule en fonction du type ou du jour
        // ✅ Couleur modifiable
        public string Color { get; set; } = "#e0e0e0";
    }
}
