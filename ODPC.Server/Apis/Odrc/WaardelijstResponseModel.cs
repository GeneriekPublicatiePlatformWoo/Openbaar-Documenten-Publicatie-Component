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
    }
}
