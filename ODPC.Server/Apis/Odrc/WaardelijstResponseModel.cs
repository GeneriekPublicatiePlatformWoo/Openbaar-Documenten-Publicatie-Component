using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ODPC.Apis.Odrc
{
    public class WaardelijstResponseModel
    {

       
            [JsonPropertyName("uuid")]
            public required string Id { get; set; }
            [JsonPropertyName("naam")]
            public required string Name { get; set; }

        //// dit zal een model per soort lijst worden. thema's zullen bijvoorbeeld genest zijn.
        //// dus dat hier nu alles default 
        //    [DefaultValue("INFORMATIECATEGORIE")]
        //[Jso]
        //    public string Type { get; set; } = "INFORMATIECATEGORIE";
    
    }
}
