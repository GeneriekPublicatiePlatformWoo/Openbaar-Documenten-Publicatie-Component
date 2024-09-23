using System.ComponentModel.DataAnnotations;

namespace ODPC.Features.AuditregelOverzicht
{
    public class AuditregelOverzichtRequest
    {
        [Range(1, double.PositiveInfinity)]
        public int? Page { get; set; }
        public bool? Geslaagd { get; set; }
        public Guid? ResourceUuid { get; set; }
        public string? GebruikersId { get; set; }
    }
}
