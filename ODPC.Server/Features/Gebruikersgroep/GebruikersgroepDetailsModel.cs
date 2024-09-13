namespace ODPC.Features.Gebruikersgroep
{
    public class GebruikersgroepDetailsModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required IEnumerable<string> GekoppeldeWaardelijsten { get; set; }
    }
}
