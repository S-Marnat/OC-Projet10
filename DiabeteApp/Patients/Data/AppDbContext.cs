using Microsoft.EntityFrameworkCore;
using Patients.Entities;

namespace Patients.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // -- Seed Data --
            modelBuilder.Entity<Patient>().HasData(
                new Patient {
                    Id = 1,
                    Prenom = "Test",
                    Nom = "TestNone",
                    DateDeNaissance = new DateTime(1966, 12, 31),
                    Genre = "F",
                    Adresse = "1 Brookside St",
                    Telephone = "100-222-3333"
                },
                new Patient {
                    Id = 2,
                    Prenom = "Test",
                    Nom = "TestBorderline",
                    DateDeNaissance = new DateTime(1945, 06, 24),
                    Genre = "M",
                    Adresse = "2 High St",
                    Telephone = "200-333-4444"
                },
                new Patient
                {
                    Id = 3,
                    Prenom = "Test",
                    Nom = "TestInDanger",
                    DateDeNaissance = new DateTime(2004, 06, 18),
                    Genre = "M",
                    Adresse = "3 Club Road",
                    Telephone = "300-444-5555"
                },
                new Patient
                {
                    Id = 4,
                    Prenom = "Test",
                    Nom = "TestEarlyOnset",
                    DateDeNaissance = new DateTime(2002, 06, 28),
                    Genre = "F",
                    Adresse = "4 Valley Dr",
                    Telephone = "400-555-6666"
                }
            );
        }
    }
}
