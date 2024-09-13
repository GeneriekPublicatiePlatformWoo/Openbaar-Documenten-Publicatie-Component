using System.ComponentModel.DataAnnotations;

namespace ODPC.Data.Entities
{
    public class Gebruikersgroep
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

    }
}
