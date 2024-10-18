using System.Text.Json;

namespace ODPC.Config
{
    public static class JsonSerialization
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
