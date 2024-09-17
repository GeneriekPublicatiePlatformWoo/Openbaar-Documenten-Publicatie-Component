using System.ComponentModel.DataAnnotations;

namespace ODPC.Data.Entities
{
    public class GebruikersgroepWaardelijst
    {  
        public Gebruikersgroep? Gebruikersgroep { get; set; }
        public required Guid GebruikersgroepId { get; set; }
        public required string WaardelijstId { get; set; }
    }
}
