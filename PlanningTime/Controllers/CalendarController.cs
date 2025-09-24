using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace PlanningTime.Controllers
{
    public class CalendarController : Controller
    {
        private readonly GraphServiceClient _graphClient;

        public CalendarController(GraphServiceClient graphClient)
        {
            _graphClient = graphClient;
        }

        public async Task<IActionResult> CreateEvent()
        {
            var @event = new Event
            {
                Subject = "Réunion PlanningTime",
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = "Ceci est un test depuis l'appli Planning."
                },
                Start = new DateTimeTimeZone
                {
                    DateTime = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                    TimeZone = "Europe/Paris"
                },
                End = new DateTimeTimeZone
                {
                    DateTime = DateTime.Now.AddHours(2).ToString("yyyy-MM-ddTHH:mm:ss"),
                    TimeZone = "Europe/Paris"
                },
                Location = new Location
                {
                    DisplayName = "Bureau"
                }
            };

            await _graphClient.Me.Events.PostAsync(@event); // ✅ Plus de .Request()

            return Content("Événement créé dans Outlook !");
        }
    }
}
