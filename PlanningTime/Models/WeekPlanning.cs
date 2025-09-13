namespace PlanningTime.Models
{
    public class WeekPlanning
    {
        // Chaque semaine contient 7 jours
        public List<PlanningDay> Days { get; set; } = new List<PlanningDay>();
    }
}
