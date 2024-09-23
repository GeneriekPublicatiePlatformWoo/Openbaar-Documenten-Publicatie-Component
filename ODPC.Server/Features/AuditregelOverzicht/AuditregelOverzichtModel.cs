using System.Net;
using System.Text.Json.Nodes;
using ODPC.Data.Entities;

namespace ODPC.Features.AuditregelOverzicht
{
    public class AuditregelOverzichtWijzingModel
    {
        public JsonNode? Oud { get; set; }
        public JsonNode? Nieuw { get; set; }
    }

    public class AuditregelOverzichtModel
    {
        public DateTimeOffset Aanmaakdatum { get; set; }
        public required AuditregelActie Actie { get; set; }
        public required string GebruikersId { get; set; }
        public required string Resource { get; set; }
        public required Guid ResourceUuid { get; set; }
        public required AuditregelOverzichtWijzingModel Wijziging { get; set; }
        public required HttpStatusCode Resultaat { get; set; }
        public required string ResultaatWeergave { get; set; }
        public bool Geslaagd { get; set; }
        public Guid Uuid { get; set; }
        public string? ResourceWeergave { get; set; }
        public string? ActieWeergave { get; set; }
    }
}
