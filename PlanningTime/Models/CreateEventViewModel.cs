using System.ComponentModel.DataAnnotations;

namespace PlanningTime.Models
{
    public class CreateEventViewModel
    {
        public int EventTypeId { get; set; } // Ex : Congé payé

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string? Motif { get; set; }
        public IFormFile? Justificatif { get; set; }
    }

}
