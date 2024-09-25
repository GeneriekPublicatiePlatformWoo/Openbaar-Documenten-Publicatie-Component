using Microsoft.EntityFrameworkCore;
using ODPC.Data.Entities;

namespace ODPC.Data
{
    public class OdpcDbContext : DbContext
    {
        public OdpcDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Gebruikersgroep> Gebruikersgroepen { get; set; }
        public DbSet<GebruikersgroepWaardelijst> GebruikersgroepWaardelijsten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Gebruikersgroep>().HasKey(t => new { t.Uuid });
            modelBuilder.Entity<GebruikersgroepWaardelijst>().HasKey(t => new { t.GebruikersgroepUuid, t.WaardelijstId });

            // Seed data, 
            // todo verwijderen zodra groepen via de applicatie aangemaakt kunnen worden
            modelBuilder.Entity<Gebruikersgroep>().HasData(
                new Gebruikersgroep { Uuid = Guid.Parse("d3da5277-ea07-4921-97b8-e9a181390c76"), Naam = "Groep 1" },
                new Gebruikersgroep { Uuid = Guid.Parse("8f939b51-dad3-436d-a5fa-495b42317d64"), Naam = "Groep 2" },
                new Gebruikersgroep { Uuid = Guid.Parse("0e7a0023-423a-421a-8700-359232fef584"), Naam = "Groep 3" }
           );
        }
    }
}
