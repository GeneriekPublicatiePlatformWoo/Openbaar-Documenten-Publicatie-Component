using System.ComponentModel.DataAnnotations;

namespace ODPC.Data.Entities
{
    public class GebruikersgroepWaardelijst
    {  
        public required Gebruikersgroep Gebruikersgroep { get; set; }
        public Guid GebruikersgroepId { get; set; }
        public required string WaardelijstId { get; set; }
    }
}
