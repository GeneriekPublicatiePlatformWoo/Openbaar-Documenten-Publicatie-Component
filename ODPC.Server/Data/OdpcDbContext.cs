using System.Text.Json.Nodes;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<Auditregel> Auditregels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gebruikersgroep>().HasKey(t => new { t.Id });
            modelBuilder.Entity<GebruikersgroepWaardelijst>().HasKey(t => new { t.GebruikersgroepId, t.WaardelijstId });
            var auditregel = modelBuilder.Entity<Auditregel>();
            auditregel.Property(x => x.Aanmaakdatum).HasDefaultValueSql("now()");
            auditregel.HasKey(x => x.Uuid);
            auditregel.HasIndex(x => new { x.GebruikersId });
            auditregel.HasIndex(x => new { x.ResourceUuid });
            auditregel.HasIndex(x => new { x.Resource });
            auditregel.HasIndex(x => new { x.Geslaagd });
            auditregel.OwnsOne(x => x.Wijzigingen);

            // Seed data, 
            // todo verwijderen zodra groepen via de applicatie aangemaakt kunnen worden
            modelBuilder.Entity<Gebruikersgroep>().HasData(
                new Gebruikersgroep { Id = Guid.Parse("d3da5277-ea07-4921-97b8-e9a181390c76"), Name = "Groep 1" },
                new Gebruikersgroep { Id = Guid.Parse("8f939b51-dad3-436d-a5fa-495b42317d64"), Name = "Groep 2" },
                new Gebruikersgroep { Id = Guid.Parse("0e7a0023-423a-421a-8700-359232fef584"), Name = "Groep 3" }
           );

        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Converteer JsonNode properties naar een byte array.
            // Postgres ondersteunt ook JsonDocument properties, maar dat is ingewikkelder met unit tests.
            // Zolang we niet hoeven te queryen binnen de JsonNode is dit voldoende.
            configurationBuilder.Properties<JsonNode>().HaveConversion<JsonNodeValueConverter>();
            base.ConfigureConventions(configurationBuilder);
        }

        private class JsonNodeValueConverter : ValueConverter<JsonNode?, byte[]>
        {
            public JsonNodeValueConverter() : base(
                x => x == null
                    ? Array.Empty<byte>()
                : JsonSerializer.SerializeToUtf8Bytes(x, s_webJsonSerializerOptions),
                x => JsonNode.Parse(x, null, default))
            {
            }

            private static readonly JsonSerializerOptions s_webJsonSerializerOptions = new(JsonSerializerDefaults.Web);
        }
    }
}
