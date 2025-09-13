using PlanningTime.Models;

namespace PlanningTime
{
    public class UserEvent
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public DayType Type { get; set; }
    }

}
