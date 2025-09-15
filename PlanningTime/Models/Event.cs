namespace PlanningTime.Models
{
    public enum EventStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Event
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int EventTypeId { get; set; }
        public EventType EventType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? Motif { get; set; }
        public string? Justificatif { get; set; }

        public EventStatus Status { get; set; } = EventStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? ValidatedById { get; set; }
        public User? ValidatedBy { get; set; }
        public DateTime? ValidatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

}
