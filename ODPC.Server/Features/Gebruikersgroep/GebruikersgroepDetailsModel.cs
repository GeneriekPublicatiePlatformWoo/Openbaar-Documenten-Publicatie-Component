namespace ODPC.Features.Gebruikersgroep
{
    public class GebruikersgroepDetailsModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        //Id's van de waardelijsten die gebruikt mogen worden binnen deze gebruikersgroep
        public required IEnumerable<string> GekoppeldeWaardelijsten { get; set; }
    }
}
