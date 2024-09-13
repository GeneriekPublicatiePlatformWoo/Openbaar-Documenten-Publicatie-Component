using Microsoft.EntityFrameworkCore;
using ODPC;
using ODPC.Data.Entities;

namespace ODPC.Data
{
    public class OdpcDbContext : DbContext
    {
        public OdpcDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public DbSet<Gebruikersgroep> Gebruikersgroepen { get; set; }
        public DbSet<GebruikersgroepWaardelijst> GebruikersgroepWaardelijsten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Gebruikersgroep>().HasKey(t => new { t.Id });
            modelBuilder.Entity<GebruikersgroepWaardelijst>().HasKey(t => new { t.GebruikersgroepId, t.WaardelijstId });


            // Seed data 
            // todo verwijderen
            modelBuilder.Entity<WeatherForecast>().HasData(
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TemperatureC = 20, Summary = "Mild" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2)), TemperatureC = 25, Summary = "Warm" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(3)), TemperatureC = 15, Summary = "Cool" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(4)), TemperatureC = 30, Summary = "Hot" },
                new WeatherForecast { Guid = Guid.NewGuid(), Date = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), TemperatureC = 10, Summary = "Chilly" }
            );

            // Seed data, 
            // todo verwijderen zodra groepen via de applicatie aangemaakt kunnen worden
            modelBuilder.Entity<Gebruikersgroep>().HasData(
                new Gebruikersgroep { Id = Guid.Parse("d3da5277-ea07-4921-97b8-e9a181390c76"), Name = "Groep 1" },
                new Gebruikersgroep { Id = Guid.Parse("8f939b51-dad3-436d-a5fa-495b42317d64"), Name = "Groep 2" },
                new Gebruikersgroep { Id = Guid.Parse("0e7a0023-423a-421a-8700-359232fef584"), Name = "Groep 3" }
           );

        }
    }
}
