using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentenOverzicht
{
    [ApiController]
    public class DocumentenOverzichtController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        private readonly IOdrcClientFactory _clientFactory = clientFactory;

        [HttpGet("api/v1/documenten")]
        public async Task<IActionResult> Get([FromQuery] string publicatie, string? page, CancellationToken token)
        {
            // documenten ophalen uit het ODRC
            var client = _clientFactory.Create("Documenten ophalen");
            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>("/api/v1/documenten?publicatie=" + publicatie + "&page=" + page, token);
            if (json != null)
            {
                json.Previous = GetPathAndQuery(json.Previous);
                json.Next = GetPathAndQuery(json.Next);
            }
            return Ok(json);
        }

        private static string? GetPathAndQuery(string? url) => Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out var uri)
            ? uri.PathAndQuery
            : url;
    }
}
