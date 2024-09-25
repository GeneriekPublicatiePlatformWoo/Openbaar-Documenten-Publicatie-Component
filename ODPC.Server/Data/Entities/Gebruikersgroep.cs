using System.ComponentModel.DataAnnotations;

namespace ODPC.Data.Entities
{
    public class Gebruikersgroep
    {
        public Guid Uuid { get; set; }
        public required string Naam { get; set; }
        public string? Omschrijving { get; set; }
    }
}
