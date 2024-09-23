using System.Text.Json;
using System.Text.Json.Nodes;

namespace ODPC.Features
{
    public static class JsonNodeExtensions
    {
        private static readonly JsonSerializerOptions s_webJsonSerializerOptions = new(JsonSerializerDefaults.Web);

        public static JsonNode ToJsonNode<T>(this T value) => JsonSerializer.SerializeToNode(value, s_webJsonSerializerOptions)!;
    }
}
