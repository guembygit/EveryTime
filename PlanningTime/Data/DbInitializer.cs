using PlanningTime.Models;
using System.Security.Cryptography;
using System.Text;

namespace PlanningTime.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PlanningDbContext context)
        {
            context.Database.EnsureCreated();

            // Seed des profils
            if (!context.Profiles.Any())
            {
                var profiles = new List<Profile>
                {
                    new Profile { Name = "Admin", Description = "Administrateur de l’application" },
                    new Profile { Name = "Employé", Description = "Utilisateur standard" }
                };
                context.Profiles.AddRange(profiles);
                context.SaveChanges();
            }

            // Seed de l'utilisateur admin
            if (!context.Users.Any(u => u.Profile.Name == "Admin"))
            {
                var adminProfile = context.Profiles.First(p => p.Name == "Admin");

                var admin = new User
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "admin@planning.com",
                    PasswordHash = HashPassword("Admin@123"),
                    HireDate = DateTime.Now,
                    IsActive = true,
                    ProfileId = adminProfile.Id
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }

            // Seed des EventTypes
            if (!context.EventTypes.Any())
            {
                var eventTypes = new List<EventType>
                {
                    new EventType { Name = "Congé", IsAbsence = true },
                    new EventType { Name = "Réunion", IsAbsence = false },
                    new EventType { Name = "Formation", IsAbsence = false }
                };
                context.EventTypes.AddRange(eventTypes);
                context.SaveChanges();
            }
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
