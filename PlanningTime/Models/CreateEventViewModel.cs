namespace PlanningTime.Models
{
    public class CreateEventViewModel
    {
        public int EventTypeId { get; set; } // Ex : Congé payé
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Motif { get; set; }
        public IFormFile? Justificatif { get; set; }
    }

}
