using System.Collections.Generic;
using System.Reflection.Emit;

//USING A AJOUTER
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Xml;

namespace PlanningTime.Models
{
    public class PlanningDbContext : DbContext
    {
        public PlanningDbContext(DbContextOptions<PlanningDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Profile>().HasData(
                new Profile { Id = 1, Name = "Admin" },
                new Profile { Id = 2, Name = "User" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FirstName = "Exemple", ProfileId = 1, IsActive = true },
                new User { Id = 2, LastName = "Autre", ProfileId = 2, IsActive = true }
            );*/

            // Relations et contraintes
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.ProfileId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(t => t.Events)
                .HasForeignKey(e => e.EventTypeId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.ValidatedBy)
                .WithMany()
                .HasForeignKey(e => e.ValidatedById);
        }
    }

}
