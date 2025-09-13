namespace PlanningTime.Services
{
    public class HolidayService
    {
        public List<DateTime> GetHolidays(int year)
        {
            return new List<DateTime>
        {
            new DateTime(year, 1, 1),   // Jour de l'an
            new DateTime(year, 5, 1),   // Fête du travail
            new DateTime(year, 5, 8),   // Victoire 1945
            new DateTime(year, 7, 14),  // Fête nationale
            new DateTime(year, 8, 15),  // Assomption
            new DateTime(year, 11, 1),  // Toussaint
            new DateTime(year, 11, 11), // Armistice
            new DateTime(year, 12, 25), // Noël
        };
        }
    }

}
