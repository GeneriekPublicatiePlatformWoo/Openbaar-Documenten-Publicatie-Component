using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using ODPC.Apis.Odrc;

namespace ODPC.Features.Documenten.DocumentenOverzicht
{
    [ApiController]
    public class DocumentenOverzichtController(IOdrcClientFactory clientFactory) : ControllerBase
    {
        [HttpGet("api/v1/documenten")]
        public async Task<IActionResult> Get([FromQuery] string publicatie, string? page, CancellationToken token)
        {
            // documenten ophalen uit het ODRC
            using var client = clientFactory.Create("Documenten ophalen");
            var json = await client.GetFromJsonAsync<PagedResponseModel<JsonNode>>("/api/v1/documenten?publicatie=" + publicatie + "&page=" + page, token);
            
            if (json != null)
            {
                json.Previous = UrlHelper.GetPathAndQuery(json.Previous);
                json.Next = UrlHelper.GetPathAndQuery(json.Next);
            }
            return Ok(json);
        }
    }
}
