using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PlanningTime.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string ImageUser { get; set; } = "";
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int ProfileId { get; set; }
        public Profile? Profile { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }

}
