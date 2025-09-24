namespace PlanningTime.Models
{
    public class UserPlanning
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }  // Nom + prénom
        public string ImageUser { get; set; }  // Nom + prénom

        public List<PlanningDay> Days { get; set; } = new List<PlanningDay>();

        // Liste des semaines du mois
        public List<WeekPlanning> Weeks { get; set; } = new List<WeekPlanning>();
        //public List<PlanningWeek> Weeks { get; set; } = new();
        public int EventId { get; set; }
        public int EventTypeId { get; set; }
    }

}
