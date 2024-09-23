using System.Net;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ODPC.Data.Entities
{
    public class AuditregelWijziging
    {
        public JsonNode? Oud { get; set; }
        public JsonNode? Nieuw { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter<AuditregelActie>))]
    public enum AuditregelActie
    {
        create,
        list,
        retrieve,
        destroy,
        update,
        partial_update
    }

    public class Auditregel
    {
        public Guid Uuid { get; private set; }
        public DateTimeOffset Aanmaakdatum { get; private set; }
        public required AuditregelActie Actie { get; set; }
        public required string ActieWeergave { get; set; }
        public required string GebruikersId { get; set; }
        public required string Resource { get; set; }
        public string? ResourceWeergave { get; set; }
        public required Guid ResourceUuid { get; set; }
        public required AuditregelWijziging Wijzigingen { get; set; }
        public required string ResultaatWeergave { get; set; }
        public required HttpStatusCode Resultaat { get; set; }
        public bool Geslaagd { get; set; }
    }
}
