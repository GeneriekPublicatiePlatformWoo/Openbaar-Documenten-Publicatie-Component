namespace ODPC.Features.Gebruikersgroep.GebruikersgroepBijwerken
{
    public class GebruikersgroepUpsertModel
    {
        public required string Naam { get; set; }
        public string? Omschrijving { get; set; }
        public required List<string> GekoppeldeWaardelijsten { get; set; }
        public required List<string> GekoppeldeGebruikers { get; set; }
    }
}
